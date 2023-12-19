using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        #region IOrderRepository Members

        public new async Task<Order> GetByIdAsync(int orderId)
        {
            return await DbContext.Set<Order>()
                .Include(i => i.Items)
                .SingleOrDefaultAsync(order => order.Id == orderId);
        }

        #endregion
    }
}
