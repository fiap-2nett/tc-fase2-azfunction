using System.Collections.Generic;

using TechChallenge.Application.Core.Messaging;
using TechChallenge.Application.Orders.Contracts;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderCommand(string CustomerEmail, IEnumerable<OrderItem> Items)
        : ICommand<Result<int?>>;
}
