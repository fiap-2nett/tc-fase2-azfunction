using System.Threading.Tasks;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain.Repositories
{
    public interface IProductRepository
    {
        #region IProductRepository Members

        Task<Product> GetByIdAsync(int productId);

        #endregion
    }
}
