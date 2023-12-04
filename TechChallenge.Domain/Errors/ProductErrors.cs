using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Domain.Errors
{
    public static class ProductErrors
    {
        public static Error NameIsRequired = new Error(
            "Product.NameIsRequired",
            "The product name is required.");

        public static Error InvalidPrice = new Error(
            "Product.InvalidPrice",
            "The product price is invalid.");

        public static Error NegativeQuantity = new Error(
            "Product.NegativeQuantity",
            "The product quantity cannot be negative.");
    }
}
