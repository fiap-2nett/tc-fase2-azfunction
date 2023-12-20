using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TechChallenge.Domain.Errors;
using TechChallenge.Domain.Extensions;
using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        #region Constants

        public const int MaxLength = 256;
        private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9](\.[a-z][a-z\.]*[a-z])+$";

        #endregion

        #region Read-Only Fields

        private static readonly Lazy<Regex> EmailFormatRegex = new Lazy<Regex>(()
            => new Regex(EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

        #endregion

        #region Properties

        public string Value { get; }

        #endregion

        #region Constructors

        private Email(string value) => Value = value;

        #endregion

        #region Factory Methods

        public static Result<Email> Create(string email)
            => Result.Create(email, DomainErrors.Email.NullOrEmpty)
                .Ensure(email => !email.IsNullOrWhiteSpace(), DomainErrors.Email.NullOrEmpty)
                .Ensure(email => email.Length <= MaxLength, DomainErrors.Email.LongerThanAllowed)
                .Ensure(email => EmailFormatRegex.Value.IsMatch(email), DomainErrors.Email.InvalidFormat)
                .Map(email => new Email(email));

        #endregion

        #region Overriden Methods

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        #endregion

        #region Operators

        public static implicit operator string(Email email)
            => email.Value;

        #endregion
    }
}
