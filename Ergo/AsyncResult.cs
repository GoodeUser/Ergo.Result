using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ergo
{
    /// <summary>
    /// Represents the result of some asynchronous operation.
    /// 
    /// This class is a wrapper around a Task<Result> so that you
    /// can chain methods on asynchronous results as well. This class
    /// is used internally but is not meant to be consumed directly.
    /// </summary>
    public class AsyncResult : AsyncResultBase
    {
        private Task<Result> _resultTask;

        public AsyncResult(Task<Result> resultTask)
        {
            if (resultTask is null)
            {
                throw new ArgumentNullException(nameof(resultTask));
            }

            _resultTask = resultTask;
        }

        public TaskAwaiter<Result> GetAwaiter() => _resultTask.GetAwaiter();

        public static implicit operator AsyncResult(Task<Result> resultTask)
        {
            return new AsyncResult(resultTask);
        }

        public static implicit operator AsyncResult(Result result)
        {
            return new AsyncResult(Task.FromResult(result));
        }

        public static implicit operator Task<Result>(AsyncResult toTask)
        {
            return toTask.ToTask();
        }

        public override Task<Result> GetTaskResult() => _resultTask;

        public Task<Result> ToTask()
        {
            return _resultTask;
        }

        public AsyncResult OnSuccess(Func<Task<Result>> mapper) => OnSuccessA(mapper);
        private async Task<Result> OnSuccessA(Func<Task<Result>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await mapper();

            return result;
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper) => OnSuccessA(mapper);
        private async Task<Result<TOut>> OnSuccessA<TOut>(Func<Task<Result<TOut>>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await mapper();

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: false);
        }

        public AsyncResult OnSuccess(Func<Result> mapper) => OnSuccessA(mapper);
        private async Task<Result> OnSuccessA(Func<Result> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await Task.FromResult(mapper());

            return result;
        }

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Result<TOut>> mapper) => OnSuccessA(mapper);
        private async Task<Result<TOut>> OnSuccessA<TOut>(Func<Result<TOut>> mapper)
        {
            var result = await this;

            if (result.IsSuccessful)
                return await Task.FromResult(mapper());

            return result as Result<TOut> ??
                new Result<TOut>(default(TOut), result.Messages, isSuccessful: false);
        }

        public AsyncResult WithMessages(params string[] messages) => WithMessagesA(messages);
        private async Task<Result> WithMessagesA(string[] messages)
        {
            var result = await this;
            result.Messages = result.Messages.Concat(messages);
            return result;
        }
    }
}