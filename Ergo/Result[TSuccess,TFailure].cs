using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ergo
{
    public struct Result<TSuccess, TFailure> : IResult<TSuccess, TFailure>, IChainableResult<TSuccess,TFailure>
    {
        private readonly TSuccess _successValue;

        private readonly TFailure _failureValue;

        private readonly List<string> _messages;

        public IReadOnlyList<string> Messages => _messages;

        public bool IsSuccessful { get; }

        public bool IsFailure => !IsSuccessful;

        internal Result(TSuccess successValue, TFailure failureValue, IEnumerable<string> messages = null, bool isSuccessful = true)
        {
            _successValue = successValue;
            _failureValue = failureValue;
            IsSuccessful = isSuccessful;
            _messages = messages?.ToList() ?? new List<string>();
        }

        public TSuccess GetSuccessValue() => _successValue!;

        public TSuccess GetSuccessValueOrDefault(TSuccess defaultValue) =>
            IsSuccessful
                ? (_successValue ?? defaultValue)
                : defaultValue;

        public TFailure GetFailureValue() => _failureValue!;

        public TFailure GetFailureValueOrDefault(TFailure defaultValue) =>
            IsFailure
                ? (_failureValue ?? defaultValue)
                : defaultValue;

        public Result<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, Result<TOut, TFailure>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut, TFailure>(default(TOut), _failureValue, Messages, isSuccessful: false);
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut, TFailure>(default(TOut), _failureValue, Messages, isSuccessful: false);
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TOut> mapper)
        {
            if (IsSuccessful)
                return mapper(_successValue);

            return new Result<TOut, TFailure>(default(TOut), _failureValue, Messages, isSuccessful: false);
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TFailure> mapper)
        {
            if (IsSuccessful)
                return new Result<TOut, TFailure>(default(TOut), mapper(_successValue), Messages, isSuccessful: true);

            return new Result<TOut, TFailure>(default(TOut), _failureValue, Messages, isSuccessful: false);
        }

        public Result<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, Result<TOut, TFailure>> mapper)
        {
            if (IsFailure)
                return mapper(this);
            
            return this is Result<TOut, TFailure>
                ? Unsafe.As<Result<TSuccess, TFailure>, Result<TOut, TFailure>>(ref this) // only create a new allocation if necessary
                : new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, AsyncResult<TOut, TFailure>> mapper)
        {
            if (IsFailure)
                return mapper(this);
            
            return this is Result<TOut, TFailure>
                ? Unsafe.As<Result<TSuccess, TFailure>, Result<TOut, TFailure>>(ref this) // only create a new allocation if necessary
                : new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TOut> mapper)
        {
            if (IsFailure)
                return mapper(this);
            
            return this is Result<TOut, TFailure>
                ? Unsafe.As<Result<TSuccess, TFailure>, Result<TOut, TFailure>>(ref this) // only create a new allocation if necessary
                : new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TFailure> mapper)
        {
            if (IsFailure)
                return Result.Failure<TOut, TFailure>(mapper(this), Messages.ToArray());
            
            return this is Result<TOut, TFailure>
                ? Unsafe.As<Result<TSuccess, TFailure>, Result<TOut, TFailure>>(ref this) // only create a new allocation if necessary
                : new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public Result<TSuccess, TFailure> WithMessages(params string[] messages)
        {
            this._messages.AddRange(messages);
            return this;
        }
    }
}