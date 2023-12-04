using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Domain.Core.Utility;
using TechChallenge.Domain.Errors;

namespace TechChallenge.Domain.Entities
{
    public sealed class Product : AggregateRoot
    {
        #region Properties

        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        #endregion

        #region Constructors

        private Product()
        { }

        public Product(string name, decimal price, int quantity)
        {
            Ensure.NotEmpty(name, ProductErrors.NameIsRequired.Message, nameof(name));
            Ensure.GreaterThanOrEqual(price, 0.01M, ProductErrors.InvalidPrice.Message, nameof(price));
            Ensure.GreaterThanOrEqual(quantity, 0, ProductErrors.NegativeQuantity.Message, nameof(quantity));

            Name = name;
            Price = price;
            Quantity = quantity;
        }

        #endregion
    }
}
