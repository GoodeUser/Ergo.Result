using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ergo
{
    public abstract class AsyncResultBase
    {
        public abstract Task<Result> GetTaskResult();

        private TaskAwaiter<Result> GetAwaiter() => GetTaskResult().GetAwaiter();

        public static AsyncResult operator +(AsyncResultBase a, AsyncResultBase b) =>
            Join(a, b);

        public static AsyncResult operator +(AsyncResultBase a, Result b) =>
            Join(a, b);

        public static AsyncResult Join(AsyncResultBase a, AsyncResultBase b) => JoinA(a, b);
        private static async Task<Result> JoinA(AsyncResultBase a, AsyncResultBase b)
        {
            var aResult = await a.GetTaskResult();
            var bResult = await b.GetTaskResult();

            var messages = aResult.Messages.Concat(bResult.Messages);
            var isSuccessful = aResult.IsSuccessful && bResult.IsSuccessful;
            return new Result(messages, isSuccessful);
        }

        public static AsyncResult Join(AsyncResultBase a, Result b) => JoinA(a, b);
        private static async Task<Result> JoinA(AsyncResultBase a, Result b)
        {
            var aResult = await a.GetTaskResult();
            return Result.Join(aResult, b);
        }

        public AsyncResult OnFailure(Func<Task<Result>> mapper) => OnFailureA(mapper);
        private async Task<Result> OnFailureA(Func<Task<Result>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await mapper();

            return result;
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Task<Result<TOut>>> mapper) => OnFailureA(mapper);
        private async Task<Result<TOut>> OnFailureA<TOut>(Func<Task<Result<TOut>>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await mapper();

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: true);
        }

        public AsyncResult OnFailure(Func<Result> mapper) => OnFailureA(mapper);
        private async Task<Result> OnFailureA(Func<Result> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await Task.FromResult(mapper());

            return result;
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TOut>> mapper) => OnFailureA(mapper);
        private async Task<Result<TOut>> OnFailureA<TOut>(Func<Result<TOut>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await Task.FromResult(mapper());

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: true);
        }

        public AsyncResult OnFailure(Func<IEnumerable<string>, Task<Result>> mapper) => OnFailureA(mapper);
        private async Task<Result> OnFailureA(Func<IEnumerable<string>, Task<Result>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await mapper(result.Messages);

            return result;
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<IEnumerable<string>, Task<Result<TOut>>> mapper) => OnFailureA(mapper);
        private async Task<Result<TOut>> OnFailureA<TOut>(Func<IEnumerable<string>, Task<Result<TOut>>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await mapper(result.Messages);

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: true);
        }

        public AsyncResult OnFailure(Func<IEnumerable<string>, Result> mapper) => OnFailureA(mapper);
        private async Task<Result> OnFailureA(Func<IEnumerable<string>, Result> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await Task.FromResult(mapper(result.Messages));

            return result;
        }

        public AsyncResult<TOut> OnFailure<TOut>(Func<IEnumerable<string>, Result<TOut>> mapper) => OnFailureA(mapper);
        private async Task<Result<TOut>> OnFailureA<TOut>(Func<IEnumerable<string>, Result<TOut>> mapper)
        {
            var result = await this;

            if (result.IsFailure)
                return await Task.FromResult(mapper(result.Messages));

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: true);
        }
    }
}