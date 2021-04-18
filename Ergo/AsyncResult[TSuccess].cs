using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ergo
{
    public struct AsyncResult<TSuccess> : IAsyncChainableResult<TSuccess>
    {
        private Task<Result<TSuccess>> _resultTask;

        public AsyncResult(Task<Result<TSuccess>> resultTask)
        {
            if (resultTask is null)
            {
                throw new ArgumentNullException(nameof(resultTask));
            }

            _resultTask = resultTask;
        }

        public TaskAwaiter<Result<TSuccess>> GetAwaiter() => _resultTask.GetAwaiter();

        public static implicit operator AsyncResult<TSuccess>(Task<Result<TSuccess>> resultTask)
        {
            return new AsyncResult<TSuccess>(resultTask);
        }

        public static implicit operator AsyncResult<TSuccess>(Result<TSuccess> result)
        {
            return new AsyncResult<TSuccess>(Task.FromResult(result));
        }

        public static implicit operator AsyncResult<TSuccess>(TSuccess result)
        {
            return new AsyncResult<TSuccess>(Task.FromResult(Result.Success(result)));
        }

        public static implicit operator AsyncResult<TSuccess>(Task<TSuccess> result)
        {
            var newResult = result.ContinueWith(
                r => Result.Success(r.Result),
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach,
                TaskScheduler.Default);

            return new AsyncResult<TSuccess>(newResult);
        }

        public static implicit operator Task<Result<TSuccess>>(AsyncResult<TSuccess> toTask)
        {
            return toTask.ToTask();
        }

        public Task<Result<TSuccess>> ToTask()
        {
            return _resultTask;
        }

        public AsyncResult OnSuccess(Func<TSuccess, Result> mapper) => OnSuccessA(mapper);
        public async Task<Result> OnSuccessA(Func<TSuccess, Result> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, Result<TOut>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<TSuccess, Result<TOut>> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<TSuccess, AsyncResult<TOut>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, Result<TOut, TFailure>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure>> OnSuccessA<TOut, TFailure>(Func<TSuccess, Result<TOut, TFailure>> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure>> OnSuccessA<TOut, TFailure>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, TOut> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<TSuccess, TOut> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, Task<TOut>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<TSuccess, Task<TOut>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult OnFailure(Func<Result<TSuccess>, Result> mapper) => OnFailureA(mapper);
        public async Task<Result> OnFailureA(Func<Result<TSuccess>, Result> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Result<TOut>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result<TSuccess>, Result<TOut>> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, AsyncResult<TOut>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result<TSuccess>, AsyncResult<TOut>> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, Result<TOut, TFailure>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut, TFailure>(Func<Result<TSuccess>, Result<TOut, TFailure>> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, AsyncResult<TOut, TFailure>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut, TFailure>(Func<Result<TSuccess>, AsyncResult<TOut, TFailure>> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, TOut> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result<TSuccess>, TOut> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Task<TOut>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result<TSuccess>, Task<TOut>> mapper) =>
            await (await this).OnFailure(mapper);
    }
}