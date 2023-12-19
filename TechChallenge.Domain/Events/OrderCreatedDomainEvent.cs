using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Core.Events;

namespace TechChallenge.Domain.Events
{
    public sealed record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
