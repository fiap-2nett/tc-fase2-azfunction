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

namespace TechChallenge.FunctionApp
{
    public class EntryPoint
    {
        #region Constants

        private const int ORDER_TIMEOUT_IN_MINUTES = 10;
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
                        ? nameof(SetOrderToProcessing)
                        : nameof(SetOrderToReject);
                    
                    await context.CallActivityAsync(functionName, orderId);
                }
                else
                {
                    await context.CallActivityAsync(nameof(SetOrderToReject), orderId);
                }
            }

            return order;
        }

        [FunctionName(nameof(CreateOrder))]
        public async Task<int> CreateOrder([ActivityTrigger] Order order, ILogger logger)
        {

            var response = await _sender.Send(new CreateOrderCommand(order.CustomerEmail, order.Items));
            return int.MaxValue;
        }

        [FunctionName(nameof(SetOrderToProcessing))]
        public async Task SetOrderToProcessing([ActivityTrigger] int orderId, ILogger logger)
        { }

        [FunctionName(nameof(SetOrderToReject))]
        public async Task SetOrderToReject([ActivityTrigger] int orderId, ILogger logger)
        { }



        //[FunctionName("Function1")]
        //public static async Task<List<string>> RunOrchestrator(
        //    [OrchestrationTrigger] IDurableOrchestrationContext context)
        //{
        //    var outputs = new List<string>();

        //    // Replace "hello" with the name of your Durable Activity Function.
        //    outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
        //    outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
        //    outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

        //    // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        //    return outputs;
        //}

        //[FunctionName(nameof(SayHello))]
        //public static string SayHello([ActivityTrigger] string name, ILogger log)
        //{
        //    log.LogInformation("Saying hello to {name}.", name);
        //    return $"Hello {name}!";
        //}

        //[FunctionName("Function1_HttpStart")]
        //public static async Task<HttpResponseMessage> HttpStart(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        //    [DurableClient] IDurableOrchestrationClient starter,
        //    ILogger log)
        //{
        //    // Function input comes from the request content.
        //    string instanceId = await starter.StartNewAsync("Function1", null);

        //    log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        //    return starter.CreateCheckStatusResponse(req, instanceId);
        //}
    }
}
