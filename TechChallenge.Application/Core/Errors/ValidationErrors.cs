using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Application.Core.Errors
{
    internal static class ValidationErrors
    {
        internal static class CreateOrder
        {
            internal static Error CustomerEmailIsRequired => new Error(
                "CreateOrder.CustomerEmailIsRequired",
                "The CustomerEmail is required.");

            internal static Error ItemsIsRequired => new Error(
                "CreateOrder.ItemsIsRequired",
                "At least one order item is required.");

            internal static Error ProductIdIsRequired => new Error(
                "CreateOrder.ProductIdIsRequired",
                "The ProductId is required.");

            internal static Error QuantityGreaterThanZero => new Error(
                "CreateOrder.QuantityGreaterThanZero",
                "The Quantity of item must be greater than a zero.");

            internal static Error DuplicatedItems => new Error(
                "CreateOrder.DuplicatedItems",
                "There are duplicate products in the order item list.");
        }
    }
}
