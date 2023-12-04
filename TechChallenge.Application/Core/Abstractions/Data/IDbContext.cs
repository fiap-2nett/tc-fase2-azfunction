using System.Threading;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Application.Core.Abstractions.Data
{
    public interface IDbContext
    {
        #region IDbContext Members

        DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity;

        Task<TEntity> GetBydIdAsync<TEntity>(int entityId)
            where TEntity : Entity;

        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;

        void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity;

        void Remove<TEntity>(TEntity entity)
            where TEntity : Entity;

        Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default);

        #endregion
    }
}
