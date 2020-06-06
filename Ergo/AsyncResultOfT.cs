using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ergo
{
    /// <summary>
    /// Represents the result of some asynchronous operation.
    /// This class is a wrapper around a Task<Result<T>> so that you
    /// can chain methods on asynchronous results as well. This class
    /// is used internally but is not meant to be consumed directly.
    /// </summary>
    public class AsyncResult<T> : AsyncResultBase
    {
        private Task<Result<T>> _resultTask;

        public AsyncResult(Task<Result<T>> resultTask)
        {
            if (resultTask is null)
            {
                throw new ArgumentNullException(nameof(resultTask));
            }

            _resultTask = resultTask;
        }

        public TaskAwaiter<Result<T>> GetAwaiter() => _resultTask.GetAwaiter();

        public static implicit operator AsyncResult<T>(Task<Result<T>> resultTask)
        {
            return new AsyncResult<T>(resultTask);
        }

        public static implicit operator AsyncResult<T>(Result<T> result)
        {
            return new AsyncResult<T>(Task.FromResult(result));
        }

        public static implicit operator AsyncResult<T>(T result)
        {
            return new AsyncResult<T>(Task.FromResult(Result.Success(result)));
        }

        public static implicit operator Task<Result<T>>(AsyncResult<T> toTask)
        {
            return toTask.ToTask();
        }

        public override async Task<Result> GetTaskResult() => await _resultTask;

        public Task<Result<T>> ToTask()
        {
            return _resultTask;
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<T, Task<Result<TOut>>> mapper) => OnSuccessA(mapper);
        private async Task<Result<TOut>> OnSuccessA<TOut>(Func<T, Task<Result<TOut>>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await mapper(result.Value);

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: false);
        }

        public AsyncResult OnSuccess(Func<T, Task<Result>> mapper) => OnSuccessA(mapper);
        private async Task<Result> OnSuccessA(Func<T, Task<Result>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await mapper(result.Value);

            return result;
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<T, Result<TOut>> mapper) => OnSuccessA(mapper);
        private async Task<Result<TOut>> OnSuccessA<TOut>(Func<T, Result<TOut>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return mapper(result.Value);

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: false);
        }

        public AsyncResult OnSuccess(Func<T, Result> mapper) => OnSuccessA(mapper);
        private async Task<Result> OnSuccessA(Func<T, Result> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return mapper(result.Value);

            return result;
        }

        public AsyncResult<T> WithMessages(params string[] messages) => WithMessagesA(messages);
        private async Task<Result<T>> WithMessagesA(string[] messages)
        {
            var result = await this;
            result.Messages = result.Messages.Concat(messages);
            return result;
        }
    }
}