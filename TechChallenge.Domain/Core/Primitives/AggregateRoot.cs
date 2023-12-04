namespace TechChallenge.Domain.Core.Primitives
{
    public abstract class AggregateRoot : Entity        
    {
        #region Constructors

        protected AggregateRoot()
        { }

        protected AggregateRoot(int idAggregateRoot)
            : base(idAggregateRoot)
        { }

        #endregion
    }
}
