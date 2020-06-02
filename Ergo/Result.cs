using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ergo
{
    /// <summary>
    /// Represents the result of some operation that may fail.
    /// If the operation fails you can add a list of messages about the failure.
    /// </summary>
    public class Result
    {
        public bool IsSuccessful { get; private set; }
        public bool IsFailure => !IsSuccessful;
        public IEnumerable<string> Messages { get; internal set; }

        internal Result(IEnumerable<string> messages = null, bool isSuccessful = true)
        {
            IsSuccessful = isSuccessful;
            Messages = messages ?? Enumerable.Empty<string>();
        }

        public static Result Success()
        {
            return new Result(isSuccessful: true);
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, isSuccessful: true);
        }

        public static Result Failure(string failure)
        {
            var failures = new[] { failure };
            return new Result(failures, isSuccessful: false);
        }

        public static Result Failure(params string[] messages)
        {
            return new Result(messages, isSuccessful: false);
        }

        public static Result Failure()
        {
            return new Result(null, isSuccessful: false);
        }

        public static Result<TOut> Failure<TOut>(string failure)
        {
            var failures = new[] { failure };
            return new Result<TOut>(default(TOut), failures, isSuccessful: false);
        }

        public static Result<TOut> Failure<TOut>(params string[] messages)
        {
            return new Result<TOut>(default(TOut), messages, isSuccessful: false);
        }

        public static Result<TOut> Failure<TOut>()
        {
            return new Result<TOut>(default(TOut), null, isSuccessful: false);
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

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: false);
        }

        public AsyncResult OnSuccess(Func<Task<Result>> mapper)
        {
            if (IsSuccessful)
                return mapper();

            return this;
        }

        public Result OnFailure(Func<Result> mapper)
        {
            if (IsFailure)
                return mapper();

            return this;
        }

        public Result<TOut> OnFailure<TOut>(Func<Result<TOut>> mapper)
        {
            if (IsFailure)
                return mapper();

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Task<Result<TOut>>> mapper)
        {
            if (IsFailure)
                return mapper();

            return this as Result<TOut> ??
                new Result<TOut>(default(TOut), Messages, isSuccessful: true);
        }

        public AsyncResult OnFailure(Func<Task<Result>> mapper)
        {
            if (IsFailure)
                return mapper();

            return this;
        }

        public static Result operator +(Result a, Result b) => Join(a, b);

        public static Task<Result> operator +(Result a, Task<Result> b) =>
            AsyncResult.Join((AsyncResult)a, (AsyncResult)b);

        public static Result Join(params Result[] all)
        {
            var messages = all.SelectMany(result => result.Messages);
            var isSuccessful = all.All(result => result.IsSuccessful);
            return new Result(messages, isSuccessful);
        }

        public Result Join(Result other) =>
            Result.Join(other);

        public Result WithMessages(params string[] messages)
        {
            this.Messages = this.Messages.Concat(messages);
            return this;
        }
    }
}