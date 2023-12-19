using MediatR;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TechChallenge.Application.Dtos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using TechChallenge.Application.Orders.Commands.CreateOrder;
using TechChallenge.Application.Orders.Commands.AcceptOrder;
using TechChallenge.Application.Orders.Commands.RejectOrder;

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

        #endregion

        #region Constructors

        public EntryPoint(ISender sender)
        {
            _sender = sender ?? throw new System.ArgumentNullException(nameof(sender));
        }

        #endregion

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
        public async Task<Order> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var order = context.GetInput<Order>();
            var orderId = await context.CallActivityAsync<int>(nameof(CreateOrder), order);

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

            return order;
        }

        [FunctionName(nameof(CreateOrder))]
        public async Task<int> CreateOrder([ActivityTrigger] Order order, ILogger logger)
        {
            logger.LogInformation($"Creating order.");
            var result = await _sender.Send(new CreateOrderCommand(order.CustomerEmail, order.Items));

            logger.LogInformation($"Order created with ID = '{result.Value}'.");

            return result.Value;
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
    }
}
