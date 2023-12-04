using System.Threading;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Persistence.Extensions;
using TechChallenge.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore.Storage;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence
{
    public sealed class EFContext : DbContext, IDbContext, IUnitOfWork
    {
        #region Constructors

        public EFContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        #endregion

        #region  IDbContext Members

        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity            
            => base.Set<TEntity>();

        public async Task<TEntity> GetBydIdAsync<TEntity>(int entityId)
            where TEntity : Entity            
            => await Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(entityId));

        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity            
            => Set<TEntity>().Add(entity);

        public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity            
            => Set<TEntity>().AddRange(entities);

        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity            
            => Set<TEntity>().Remove(entity);

        public Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
            => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

        #endregion

        #region IUnitOfWork Members

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => Database.BeginTransactionAsync(cancellationToken);

        #endregion

        #region Overriden Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetDefaultColumnTypes();
            modelBuilder.RemoveCascadeConvention();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplySeedConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
