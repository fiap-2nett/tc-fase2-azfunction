using System;

namespace TechChallenge.Domain.Core.Primitives.Result
{
    public class Result
    {        
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }
        
        public bool IsSuccess { get; }
        
        public bool IsFailure => !IsSuccess;
        
        public Error Error { get; }
        
        public static Result Success() => new Result(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
        
        public static Result<TValue> Create<TValue>(TValue value, Error error)
            where TValue : class
            => value is null ? Failure<TValue>(error) : Success(value);
        
        public static Result Failure(Error error) => new Result(false, error);
        
        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default!, false, error);
        
        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Success();
        }
    }
}
