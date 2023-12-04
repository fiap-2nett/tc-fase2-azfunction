using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Domain.Core.Utility;
using TechChallenge.Domain.Errors;

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

        public OrderItem(int productId, decimal price, int quantity)
        {
            Ensure.GreaterThanOrEqual(productId, 0, ProductErrors.NameIsRequired.Message, nameof(productId));
            Ensure.GreaterThanOrEqual(price, 0M, ProductErrors.InvalidPrice.Message, nameof(price));
            Ensure.GreaterThanOrEqual(quantity, 1, ProductErrors.NegativeQuantity.Message, nameof(quantity));

            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        #endregion
    }
}
