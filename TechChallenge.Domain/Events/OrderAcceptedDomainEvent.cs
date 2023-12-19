using TechChallenge.Domain.Core.Events;

namespace TechChallenge.Domain.Events
{
    public sealed record OrderAcceptedDomainEvent(int OrderId) : IDomainEvent;
}
