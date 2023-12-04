using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Domain.Errors
{
    public static class EmailErrors
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
}
