using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ergo
{
    public struct AsyncResult : IAsyncChainableResult
    {
        private readonly Task<Result> _resultTask;

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

        public Task<Result> ToTask()
        {
            return _resultTask;
        }

        public AsyncResult OnSuccess(Func<Result> mapper) => OnSuccessA(mapper);
        public async Task<Result> OnSuccessA(Func<Result> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Result<TOut>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<Result<TOut>> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<Task<Result<TOut>>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Result<TOut, TFailure>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure> > OnSuccessA<TOut, TFailure>(Func<Result<TOut, TFailure>> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Task<Result<TOut, TFailure>>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut, TFailure> > OnSuccessA<TOut, TFailure>(Func<Task<Result<TOut, TFailure>>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<TOut> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<TOut> mapper) =>
            (await this).OnSuccess(mapper);

        public AsyncResult<TOut> OnSuccess<TOut>(Func<Task<TOut>> mapper) => OnSuccessA(mapper);
        public async Task<Result<TOut>> OnSuccessA<TOut>(Func<Task<TOut>> mapper) =>
            await (await this).OnSuccess(mapper);

        public AsyncResult OnFailure(Func<Result, Result> mapper) => OnFailureA(mapper);
        public async Task<Result> OnFailureA(Func<Result, Result> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, Result<TOut>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result, Result<TOut>> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<Result<TOut>>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result, Task<Result<TOut>>> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Result<TOut, TFailure>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut, TFailure>(Func<Result, Result<TOut, TFailure>> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Task<Result<TOut, TFailure>>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut, TFailure>> OnFailureA<TOut, TFailure>(Func<Result, Task<Result<TOut, TFailure>>> mapper) =>
            await (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, TOut> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result, TOut> mapper) =>
            (await this).OnFailure(mapper);

        public AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<TOut>> mapper) => OnFailureA(mapper);
        public async Task<Result<TOut>> OnFailureA<TOut>(Func<Result, Task<TOut>> mapper) =>
            await (await this).OnFailure(mapper);
    }
}