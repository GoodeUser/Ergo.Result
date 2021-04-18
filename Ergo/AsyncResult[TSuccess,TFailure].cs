using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ergo
{
    public struct AsyncResult<TSuccess, TFailure> : IAsyncChainableResult<TSuccess, TFailure>
    {
        private Task<Result<TSuccess, TFailure>> _resultTask;

        public AsyncResult(Task<Result<TSuccess, TFailure>> resultTask)
        {
            if (resultTask is null)
            {
                throw new ArgumentNullException(nameof(resultTask));
            }

            _resultTask = resultTask;
        }

        public TaskAwaiter<Result<TSuccess, TFailure>> GetAwaiter() => _resultTask.GetAwaiter();

        public static implicit operator AsyncResult<TSuccess, TFailure>(Task<Result<TSuccess, TFailure>> resultTask)
        {
            return new AsyncResult<TSuccess, TFailure>(resultTask);
        }

        public static implicit operator AsyncResult<TSuccess, TFailure>(Task<Result<TSuccess>> resultTask)
        {
            var result = resultTask.ContinueWith(task => Result.Success<TSuccess, TFailure>(task.Result.GetSuccessValue()));
            return new AsyncResult<TSuccess, TFailure>(result);
        }

        public static implicit operator AsyncResult<TSuccess, TFailure>(Result<TSuccess, TFailure> result)
        {
            return new AsyncResult<TSuccess, TFailure>(Task.FromResult(result));
        }

        public static implicit operator AsyncResult<TSuccess, TFailure>(Result<TSuccess> result)
        {
            var newResult = Result.Success<TSuccess, TFailure>(result.GetSuccessValue());
            return new AsyncResult<TSuccess, TFailure>(Task.FromResult(newResult));
        }

        public static implicit operator AsyncResult<TSuccess, TFailure>(TSuccess result)
        {
            var newResult = Task.FromResult(Result.Success<TSuccess, TFailure>(result));
            return new AsyncResult<TSuccess, TFailure>(newResult);
        }

        public static implicit operator Task<Result<TSuccess, TFailure>>(AsyncResult<TSuccess, TFailure> toTask)
        {
            return toTask.ToTask();
        }

        public Task<Result<TSuccess, TFailure>> ToTask()
        {
            return _resultTask;
        }

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, Result<TOut, TFailure>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure>> OnSuccessA<TOut>(Func<TSuccess, Result<TOut, TFailure>> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure>> OnSuccessA<TOut>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TOut> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure>> OnSuccessA<TOut>(Func<TSuccess, TOut> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TSuccess, TFailure> OnSuccess(Func<TSuccess, TFailure> mapper)=> OnSuccessA(mapper);
        public async Task<Result<TSuccess, TFailure>> OnSuccessA(Func<TSuccess, TFailure> mapper) =>
            await (await this).OnSuccess<TSuccess>(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, Result<TOut, TFailure>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut>(Func<Result<TSuccess, TFailure>, Result<TOut, TFailure>> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, AsyncResult<TOut, TFailure>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut>(Func<Result<TSuccess, TFailure>, AsyncResult<TOut, TFailure>> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TOut> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut>(Func<Result<TSuccess, TFailure>, TOut> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TSuccess, TFailure> OnFailure(Func<Result<TSuccess, TFailure>, TFailure> mapper) => OnFailureA(mapper);
        public async Task<Result<TSuccess, TFailure>> OnFailureA(Func<Result<TSuccess, TFailure>, TFailure> mapper) =>
            await (await this).OnFailure<TSuccess>(mapper);
    }
}