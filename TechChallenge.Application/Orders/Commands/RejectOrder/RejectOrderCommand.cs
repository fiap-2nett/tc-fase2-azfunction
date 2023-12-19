using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Application.Orders.Commands.RejectOrder
{
    public sealed record RejectOrderCommand(int OrderId) : ICommand<Result>;
}
