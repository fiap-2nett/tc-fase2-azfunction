using System.Collections.Generic;

namespace TechChallenge.Application.Orders.Contracts
{
    public sealed record Order(string CustomerEmail, IEnumerable<OrderItem> Items);
}
