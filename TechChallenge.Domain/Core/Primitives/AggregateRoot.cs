using System.Collections.Generic;
using TechChallenge.Domain.Core.Events;

namespace TechChallenge.Domain.Core.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        #region Read-Only Fields

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        #endregion

        #region Properties

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        #endregion

        #region Constructors

        protected AggregateRoot()
        { }

        protected AggregateRoot(int idAggregateRoot)
            : base(idAggregateRoot)
        { }

        #endregion

        #region Public Methods

        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();

        #endregion
    }
}
