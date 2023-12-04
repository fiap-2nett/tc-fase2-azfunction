using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Repositories;
using TechChallenge.Persistence.Core.Primitives;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence.Repositories
{
    internal sealed class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        #region Constructors

        public OrderRepository(IDbContext dbContext) : base(dbContext)
        { }

        #endregion
    }
}
