using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence.Core.Primitives
{
    internal abstract class GenericRepository<TEntity>
        where TEntity : Entity
    {
        #region Properties

        protected IDbContext DbContext { get; }

        #endregion

        #region Constructors

        protected GenericRepository(IDbContext dbContext)
            => DbContext = dbContext;

        #endregion

        #region Methods

        public async Task<TEntity> GetByIdAsync(int entityId)
            => await DbContext.GetBydIdAsync<TEntity>(entityId);

        public void Insert(TEntity entity)
            => DbContext.Insert(entity);

        public void InsertRange(IReadOnlyCollection<TEntity> entities)
            => DbContext.InsertRange(entities);

        public void Update(TEntity entity)
            => DbContext.Set<TEntity>().Update(entity);

        public void Remove(TEntity entity)
            => DbContext.Remove(entity);

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            => await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
            => await DbContext.Set<TEntity>().AnyAsync(predicate);

        #endregion
    }
}
