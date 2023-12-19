using MediatR;

namespace TechChallenge.Domain.Core.Events
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    { }
}
