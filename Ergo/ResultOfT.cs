using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ergo
{
    /// <summary>
    /// Represents the result of some operation that may fail.
    /// If the operation is successful it will hold some value, otherwise
    /// you can add a list of messages about the failure.
    /// </summary>
    public class Result<T> : Result
    {
        public T Value { get; private set; }

        internal Result(T value, IEnumerable<string> messages = null, bool isSuccessful = true)
            : base(messages, isSuccessful)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value, isSuccessful: true);
        }

        public static Task<Result> operator +(Result<T> a, Task<Result<T>> b) =>
            AsyncResult.Join((AsyncResult<T>)a, (AsyncResult<T>)b);

        public Result OnSuccess(Func<T, Result> mapper) //will this
        {
            if (IsSuccessful)
                return mapper(Value);

            return this;
        }

        public Result<TOut> OnSuccess<TOut>(Func<T, Result<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper(Value);

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<T, Task<Result<TOut>>> mapper)
        {
            if (IsSuccessful)
                return mapper(Value);

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult OnSuccess(Func<T, Task<Result>> mapper)
        {
            if (IsSuccessful)
                return mapper(Value);

            return this;
        }

        public Result OnFailure(Func<T, Result> mapper)
        {
            if (IsFailure)
                return mapper(Value);

            return this;
        }

        public Result<TOut> OnFailure<TOut>(Func<T, Result<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(Value);

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<T, Task<Result<TOut>>> mapper)
        {
            if (IsFailure)
                return mapper(Value);

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult OnFailure(Func<T, Task<Result>> mapper)
        {
            if (IsFailure)
                return mapper(Value);

            return this;
        }
    }
}
