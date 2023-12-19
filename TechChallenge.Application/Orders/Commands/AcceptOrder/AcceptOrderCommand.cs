using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Application.Orders.Commands.AcceptOrder
{
    public sealed record AcceptOrderCommand(int OrderId) : ICommand<Result>;
}
