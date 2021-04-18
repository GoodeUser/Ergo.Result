using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ergo
{
    public struct Result<TSuccess> : IResult<TSuccess>, IChainableResult<TSuccess>
    {
        private readonly TSuccess _successValue;

        private readonly List<string> _messages;

        public IReadOnlyList<string> Messages => _messages;

        public bool IsSuccessful { get; }

        public bool IsFailure => !IsSuccessful;

        internal Result(TSuccess successValue, IEnumerable<string> messages = null, bool isSuccessful = true)
        {
            _successValue = successValue;
            IsSuccessful = isSuccessful;
            _messages = messages?.ToList() ?? new List<string>();
        }

        public TSuccess GetSuccessValue() => _successValue;

        public TSuccess GetSuccessValueOrDefault(TSuccess defaultValue) =>
            IsSuccessful
                ? (_successValue ?? defaultValue)
                : defaultValue;

        public Result OnSuccess(Func<TSuccess, Result> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result(Messages, isSuccessful: false);
        }

        public Result<TOut> OnSuccess<TOut>(Func<TSuccess, Result<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public Result<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, Result<TOut, TFailure>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: false);
        }

        public Result<TOut> OnSuccess<TOut>(Func<TSuccess, TOut> mapper)
        {
            if (IsSuccessful)
                return Result.Success(mapper(_successValue));

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, System.Threading.Tasks.Task<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public Result OnFailure(Func<Result<TSuccess>, Result> mapper)
        {
            if (IsFailure)
                return mapper(this);
                
            return new Result(Messages, isSuccessful: true);
        }

        public Result<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Result<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return this is Result<TOut>
                ? Unsafe.As<Result<TSuccess>, Result<TOut>>(ref this) // only create a new allocation if necessary
                : new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, AsyncResult<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(this);
                
            return this is Result<TOut>
                ? Unsafe.As<Result<TSuccess>, Result<TOut>>(ref this) // only create a new allocation if necessary
                : new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public Result<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, Result<TOut, TFailure>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            var successValue = _successValue is TOut
                ? Unsafe.As<TSuccess, TOut>(ref Unsafe.AsRef(_successValue))
                : default(TOut);

            return new Result<TOut, TFailure>(
                successValue,
                default(TFailure),
                Messages,
                isSuccessful: true);
        }

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, AsyncResult<TOut, TFailure>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            var successValue = _successValue is TOut
                ? Unsafe.As<TSuccess, TOut>(ref Unsafe.AsRef(_successValue))
                : default(TOut);

            return new Result<TOut, TFailure>(
                successValue,
                default(TFailure),
                Messages,
                isSuccessful: true);
        }

        public Result<TOut> OnFailure<TOut>(Func<Result<TSuccess>, TOut> mapper)
        {
            if (IsFailure)
                return Result.Success(mapper(this));

            var successValue = _successValue is TOut
                ? Unsafe.As<TSuccess, TOut>(ref Unsafe.AsRef(_successValue))
                : default(TOut);

            return new Result<TOut>(
                successValue,
                Messages,
                isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, System.Threading.Tasks.Task<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            var successValue = _successValue is TOut
                ? Unsafe.As<TSuccess, TOut>(ref Unsafe.AsRef(_successValue))
                : default(TOut);

            return new Result<TOut>(
                successValue,
                Messages,
                isSuccessful: true);
        }
    }
}