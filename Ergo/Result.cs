using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ergo
{
    public struct Result : IResult, IChainableResult
    {
        private readonly List<string> _messages;

        public IReadOnlyList<string> Messages => _messages;

        public bool IsSuccessful { get; }

        public bool IsFailure => !IsSuccessful;

        internal Result(IEnumerable<string> messages = null, bool isSuccessful = true)
        {
            IsSuccessful = isSuccessful;
            _messages = messages?.ToList() ?? new List<string>();
        }

        public static Result Success()
        {
            return new Result(isSuccessful: true);
        }

        public static Result<TSuccess> Success<TSuccess>(TSuccess value)
        {
            return new Result<TSuccess>(value, isSuccessful: true);
        }

        public static Result<TSuccess, TFailure> Success<TSuccess, TFailure>(TSuccess value)
        {
            return new Result<TSuccess, TFailure>(value, default(TFailure), isSuccessful: true);
        }
        
        public static Result Failure(params string[] messages)
        {
            return new Result(messages, isSuccessful: false);
        }

        public static Result Failure()
        {
            return new Result(null, isSuccessful: false);
        }

        public static Result<TSuccess> Failure<TSuccess>(params string[] messages)
        {
            return new Result<TSuccess>(default(TSuccess), messages, isSuccessful: false);
        }

        public static Result<TSuccess> Failure<TSuccess>()
        {
            return new Result<TSuccess>(default(TSuccess), null, isSuccessful: false);
        }

        public static Result<TSuccess, TFailure> Failure<TSuccess, TFailure>(TFailure failureValue, params string[] messages)
        {
            return new Result<TSuccess, TFailure>(default(TSuccess), failureValue, messages, isSuccessful: false);
        }

        public Result OnSuccess(Func<Result> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return this;
        }

        public Result<TOut> OnSuccess<TOut>(Func<Result<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public Result<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Result<TOut, TFailure>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), isSuccessful: false);
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Task<Result<TOut, TFailure>>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), isSuccessful: false);
        }

        public Result<TOut> OnSuccess<TOut>(Func<TOut> mapper)
        {
            if (IsSuccessful)
                return Result.Success(mapper());

            return new Result<TOut>(default(TOut), isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<TOut>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return new Result<TOut>(default(TOut), isSuccessful: false);
        }

        public Result OnFailure(Func<Result, Result> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result(Messages, isSuccessful: true);
        }

        public Result<TOut> OnFailure<TOut>(Func<Result, Result<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<Result<TOut>>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public Result<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Result<TOut, TFailure>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Task<Result<TOut, TFailure>>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result<TOut, TFailure>(default(TOut), default(TFailure), Messages, isSuccessful: true);
        }

        public Result<TOut> OnFailure<TOut>(Func<Result, TOut> mapper)
        {
            if (IsFailure)
                return Result.Success(mapper(this));

            return new Result<TOut>(default(TOut), isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<TOut>> mapper)
        {
            if (IsFailure)
                return mapper(this);

            return new Result<TOut>(default(TOut), isSuccessful: true);
        }
    }
}