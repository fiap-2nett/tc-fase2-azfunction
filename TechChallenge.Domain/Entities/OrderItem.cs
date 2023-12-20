using System.Threading.Tasks;

using TechChallenge.Domain.Errors;
using TechChallenge.Domain.Repositories;
using TechChallenge.Domain.Core.Utility;
using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Domain.Entities
{
    public sealed class OrderItem : Entity
    {
        #region Properties

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        #endregion

        #region Constructors

        private OrderItem()
        { }

        private OrderItem(int productId, decimal price, int quantity)
        {
            Ensure.GreaterThanOrEqual(productId, 0, DomainErrors.Product.NameIsRequired.Message, nameof(productId));
            Ensure.GreaterThanOrEqual(price, 0M, DomainErrors.Product.InvalidPrice.Message, nameof(price));
            Ensure.GreaterThanOrEqual(quantity, 1, DomainErrors.Product.NegativeQuantity.Message, nameof(quantity));

            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        #endregion

        #region Factory Methods

        public static async Task<Result<OrderItem>> CreateAsync(IProductRepository productRepository, int productId, int quantity)
        {
            var product = await productRepository.GetByIdAsync(productId);
            if (product is null)
                return Result.Failure<OrderItem>(DomainErrors.Product.NotFound);

            if (product.Quantity < quantity)
                return Result.Failure<OrderItem>(DomainErrors.Product.InsufficientStock);

            return Result.Success(new OrderItem(product.Id, product.Price, quantity));
        }

        #endregion
    }
}
