using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Domain.Errors
{
    public static class DomainErrors
    {
        public static class General
        {
            public static Error UnProcessableRequest => new Error(
                "General.UnProcessableRequest",
                "The server could not process the request.");

            public static Error ServerError => new Error(
                "General.ServerError",
                "The server encountered an unrecoverable error.");
        }

        public static class Email
        {
            public static Error NullOrEmpty => new Error(
                "Email.NullOrEmpty",
                "The email is required.");

            public static Error LongerThanAllowed => new Error(
                "Email.LongerThanAllowed",
                "The email is longer than allowed.");

            public static Error InvalidFormat => new Error(
                "Email.InvalidFormat",
                "The email format is invalid.");
        }

        public static class Product
        {
            public static Error NotFound = new Error(
                "Product.NotFound",
                "The product requested with the specified indentifier was not found.");

            public static Error NameIsRequired = new Error(
                "Product.NameIsRequired",
                "The product name is required.");

            public static Error InvalidPrice = new Error(
                "Product.InvalidPrice",
                "The product price is invalid.");

            public static Error NegativeQuantity = new Error(
                "Product.NegativeQuantity",
                "The product quantity cannot be negative.");

            public static Error InsufficientStock = new Error(
                "Product.InsufficientStock",
                "Insufficient quantity of product in stock.");
        }

        public static class Order
        {
            public static Error NotFound = new Error(
                "Order.NotFound",
                "The order requested with the specified indentifier was not found.");

            public static Error AlreadyProcessing = new Error(
                "Order.AlreadyProcessing",
                "The order request has already been processing.");

            public static Error AlreadyAccepted = new Error(
                "Order.AlreadyAccepted",
                "The order request has already been accepted.");

            public static Error AlreadyRejected => new Error(
                "Order.AlreadyRejected",
                "The order request has already been rejected.");
        }
    }
}
