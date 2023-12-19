using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Persistence.Extensions;
using TechChallenge.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence
{
    public sealed class EFContext : DbContext, IDbContext, IUnitOfWork
    {
        #region Read-Only Fields

        private readonly IMediator _mediator;

        #endregion

        #region Constructors

        internal EFContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        public EFContext(DbContextOptions dbContextOptions, IMediator mediator)
            : base(dbContextOptions)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await base.SaveChangesAsync(cancellationToken);
            if (affectedRows > 0)
                await PublishDomainEventsAsync(cancellationToken);

            return affectedRows;
        }

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

        #region Private Methods

        private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
        {
            var aggregateRoots = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
                .ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }

        #endregion
    }
}
