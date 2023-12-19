using TechChallenge.Domain.Core.Events;

namespace TechChallenge.Domain.Events
{
    public sealed record OrderRejectedDomainEvent(int OrderId) : IDomainEvent;
}
