using System;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

using TechChallenge.Domain.Errors;
using TechChallenge.FunctionApp.Contracts;
using TechChallenge.Domain.Core.Exceptions;
using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Application.Orders.Contracts;
using TechChallenge.Application.Orders.Commands.CreateOrder;
using TechChallenge.Application.Orders.Commands.AcceptOrder;
using TechChallenge.Application.Orders.Commands.RejectOrder;
using ValidationException = TechChallenge.Application.Core.Exceptions.ValidationException;

namespace TechChallenge.FunctionApp
{
    public class EntryPoint
    {
        #region Constants

        private const double ORDER_TIMEOUT_IN_MINUTES = 3;
        private const string ACCEPT_ORDER_EVENT = "AcceptOrder";

        #endregion

        #region Read-Only

        private readonly ISender _sender;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        #endregion

        #region Constructors

        public EntryPoint(ISender sender)
        {
            _sender = sender ?? throw new System.ArgumentNullException(nameof(sender));
        }

        #endregion

        #region Functions

        [FunctionName("Order")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            var body = await req.Content.ReadAsAsync<Order>(default(CancellationToken));
            var instanceId = await starter.StartNewAsync(nameof(RunOrchestrator), null, body);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName(nameof(RunOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var order = context.GetInput<Order>();
            var orderResult = await context.CallActivityAsync<(bool IsSuccess, int? OrderId, Error Error)>(nameof(CreateOrder), order);

            if (!orderResult.IsSuccess)
            {
                context.SetOutput(orderResult.Error.Message);
                return;
            }

            var orderId = orderResult.OrderId.GetValueOrDefault();

            using (var cts = new CancellationTokenSource())
            {                
                var orderTimeout = context.CreateTimer(context.CurrentUtcDateTime.AddMinutes(ORDER_TIMEOUT_IN_MINUTES), cts.Token);
                var orderAccepted = context.WaitForExternalEvent<bool>(ACCEPT_ORDER_EVENT);

                if (orderAccepted == await Task.WhenAny(orderAccepted, orderTimeout))
                {
                    cts.Cancel();

                    var functionName = orderAccepted.Result
                        ? nameof(AcceptOrder)
                        : nameof(RejectOrder);
                    
                    await context.CallActivityAsync(functionName, orderId);
                }
                else
                {
                    await context.CallActivityAsync(nameof(RejectOrder), orderId);
                }
            }

            return;
        }

        [FunctionName(nameof(CreateOrder))]
        public async Task<(bool IsSuccess, int? OrderId, Error Error)> CreateOrder([ActivityTrigger] Order order, ILogger logger)
        {
            try
            {
                logger.LogInformation($"Creating order.");
                var result = await _sender.Send(new CreateOrderCommand(order.CustomerEmail, order.Items));

                if (result.IsSuccess)                    
                    logger.LogInformation($"Order created with ID = '{result.Value}'.");
                else
                    logger.LogError(JsonSerializer.Serialize(new ErrorResponse(new[] { result.Error }), _serializerOptions));

                return (result.IsSuccess, result.Value, result.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex, logger);
            }

            return (false, default, DomainErrors.General.UnProcessableRequest);            
        }

        [FunctionName(nameof(AcceptOrder))]
        public async Task AcceptOrder([ActivityTrigger] int orderId, ILogger logger)
        {
            logger.LogInformation($"Accepting order.");
            await _sender.Send(new AcceptOrderCommand(orderId));

            logger.LogInformation($"Order with ID = '{orderId}' has accepted.");
        }

        [FunctionName(nameof(RejectOrder))]
        public async Task RejectOrder([ActivityTrigger] int orderId, ILogger logger)
        {
            logger.LogInformation($"Rejecting order.");
            await _sender.Send(new RejectOrderCommand(orderId));

            logger.LogInformation($"Order with ID = '{orderId}' has rejected.");
        }

        #endregion

        #region Private Methods

        private void HandleException(Exception exception, ILogger logger)
        {
            (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(exception);
            logger.LogError(JsonSerializer.Serialize(new ErrorResponse(errors), _serializerOptions));
        }

        private (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error>) GetHttpStatusCodeAndErrors(Exception exception)
            => exception switch
            {
                ValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors),
                DomainException domainException => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
                _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
            };

        #endregion
    }
}
