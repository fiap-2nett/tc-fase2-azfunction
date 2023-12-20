using System;
using FluentValidation;

using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Application.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        #region Extension Methods

        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error), "The error is required");

            return rule.WithErrorCode(error.Code).WithMessage(error.Message);
        }

        #endregion
    }
}
