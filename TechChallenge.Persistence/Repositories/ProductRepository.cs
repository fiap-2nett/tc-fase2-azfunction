using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Repositories;
using TechChallenge.Persistence.Core.Primitives;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence.Repositories
{
    internal sealed class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        #region Constructors

        public ProductRepository(IDbContext dbContext) : base(dbContext)
        { }

        #endregion
    }
}
