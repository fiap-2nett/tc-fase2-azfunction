using System.Collections.Generic;
using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.FunctionApp.Contracts
{
    public class ErrorResponse
    {
        #region Properties

        public IReadOnlyCollection<Error> Errors { get; }

        #endregion

        #region Constructors

        public ErrorResponse(IReadOnlyCollection<Error> errors)
            => Errors = errors;

        #endregion
    }
}
