using System.Collections.Generic;

namespace Conta360.Core.Common
{
    public class OperationResult<TValue> : OperationResult
    {
        private readonly TValue? _value;

        protected internal OperationResult(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static implicit operator OperationResult<TValue>(TValue value) => Success(value);
        public static implicit operator OperationResult<TValue>(Error error) => Failure<TValue>(error);
    }

    public class OperationResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public IReadOnlyList<Error> Errors { get; } // For multiple errors

        protected internal OperationResult(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Errors = new List<Error> { error }; // Default for single error
        }

        protected internal OperationResult(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Error = errors.FirstOrDefault() ?? Error.None; // First error for convenience
        }

        public static OperationResult Success() => new(true, Error.None);
        public static OperationResult<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static OperationResult Failure(Error error) => new(false, error);
        public static OperationResult<TValue> Failure<TValue>(Error error) => new(default, false, error);
        public static OperationResult Failure(IReadOnlyList<Error> errors) => new(false, errors);
        public static OperationResult<TValue> Failure<TValue>(IReadOnlyList<Error> errors) => new(default, false, errors);

        public static OperationResult<TValue> Create<TValue>(TValue? value)
            where TValue : class
        {
            return value is null ? Failure<TValue>(Error.NullValue) : Success(value);
        }
    }
}