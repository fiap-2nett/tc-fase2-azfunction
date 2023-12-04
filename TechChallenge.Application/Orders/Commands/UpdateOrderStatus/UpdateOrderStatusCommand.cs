using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Enumerations;

namespace TechChallenge.Application.Orders.Commands.UpdateOrderStatus
{
    public sealed record UpdateOrderStatusCommand(int OrderId, OrderStatus Status) : ICommand<UpdateOrderStatusCommand.Response>
    {
        public enum Response
        {
            Successful,
            Error
        }
    }
}
