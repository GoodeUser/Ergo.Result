using System;
using System.Threading.Tasks;

namespace Ergo
{
    public interface IAsyncChainableResult
    {
        AsyncResult OnSuccess(Func<Result> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<Result<TOut>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Task<Result<TOut, TFailure>>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<TOut> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<Task<TOut>> mapper);


        AsyncResult OnFailure(Func<Result, Result> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, Result<TOut>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<Result<TOut>>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Task<Result<TOut, TFailure>>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, TOut> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<TOut>> mapper);
    }
}
