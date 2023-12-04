using System.Collections.Generic;

namespace TechChallenge.Application.Dtos
{
    public sealed record Order(string CustomerEmail, IEnumerable<OrderItem> Items);
}
