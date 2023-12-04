using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Persistence.Core.Primitives
{
    internal interface IEntitySeedConfiguration
    {
        #region IEntitySeedConfiguration Members

        abstract IEnumerable<object> Seed();
        void Configure(ModelBuilder modelBuilder);

        #endregion
    }

    internal abstract class EntitySeedConfiguration<TEntity> : IEntitySeedConfiguration
        where TEntity : Entity
    {
        #region IEntitySeedConfiguration Members

        public abstract IEnumerable<object> Seed();

        public void Configure(ModelBuilder modelBuilder)
            => modelBuilder.Entity<TEntity>().HasData(Seed());

        #endregion
    }
}
