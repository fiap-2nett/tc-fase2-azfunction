using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using TechChallenge.Application.Core.Messaging;
using ValidationException = TechChallenge.Application.Core.Exceptions.ValidationException;

namespace TechChallenge.Application.Core.Behaviours
{
    internal sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
    {
        #region Read-Only Fields

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        #endregion

        #region Constructors

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        #endregion

        #region IPipelineBehavior Members

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IQuery<TResponse>)
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);

            return await next();
        }

        #endregion
    }
}
