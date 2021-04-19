using System;
using System.Threading.Tasks;

namespace Ergo
{
    internal interface IChainableResult
    {
        Result OnSuccess(Func<Result> mapper);
        Result<TOut> OnSuccess<TOut>(Func<Result<TOut>> mapper);
        AsyncResult OnSuccess(Func<Task<Result>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<Task<Result<TOut>>> mapper);
        Result<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<Task<Result<TOut, TFailure>>> mapper);
        Result<TOut> OnSuccess<TOut>(Func<TOut> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<Task<TOut>> mapper);


        Result OnFailure(Func<Result, Result> mapper);
        Result<TOut> OnFailure<TOut>(Func<Result, Result<TOut>> mapper);
        AsyncResult OnFailure(Func<Result, Task<Result>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<Result<TOut>>> mapper);
        Result<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result, Task<Result<TOut, TFailure>>> mapper);
        Result<TOut> OnFailure<TOut>(Func<Result, TOut> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result, Task<TOut>> mapper);
    }
}