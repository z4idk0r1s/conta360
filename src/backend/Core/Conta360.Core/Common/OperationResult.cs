using System;
using System.Collections.Generic;
using System.Linq;

namespace Conta360.Core.Common
{
    public class OperationResult<TValue> : OperationResult
    {
        private readonly TValue? _value;

        public static OperationResult<TValue> Failure(Error error)
        {
            return new OperationResult<TValue>(default, false, error);
        }

        protected internal OperationResult(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        protected internal OperationResult(TValue? value, bool isSuccess, IReadOnlyList<Error> errors)
            : base(isSuccess, errors)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access the value of a failed result.");

        public static implicit operator OperationResult<TValue>(TValue value) => Success(value);
        public static implicit operator OperationResult<TValue>(Error error) => Failure<TValue>(error);
    }

    public class OperationResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public IReadOnlyList<Error> Errors { get; }

        protected internal OperationResult(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error state combination.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Errors = new List<Error> { error };
        }

        protected internal OperationResult(bool isSuccess, IReadOnlyList<Error> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                throw new ArgumentException("Errors must contain at least one error.", nameof(errors));
            }

            IsSuccess = isSuccess;
            Errors = errors;
            Error = errors.FirstOrDefault() ?? Error.None;
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
