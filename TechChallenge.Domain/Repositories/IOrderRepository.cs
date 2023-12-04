using System.Threading.Tasks;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain.Repositories
{
    public interface IOrderRepository
    {
        #region IOrderRepository Members

        void Insert(Order order);
        Task<Order> GetByIdAsync(int orderId);

        #endregion
    }
}
