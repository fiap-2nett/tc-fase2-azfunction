using System.Collections.Generic;
using TechChallenge.Application.Core.Messaging;
using TechChallenge.Application.Dtos;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderCommand(string CustomerEmail, IEnumerable<OrderItem> Items) : ICommand<(CreateOrderCommand.Response Response, int OrderId)>
    {
        public enum Response
        {
            Successful,
            Error
        }
    }
}
