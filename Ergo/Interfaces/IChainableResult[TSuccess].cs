using System;
using System.Threading.Tasks;

namespace Ergo
{
    public interface IChainableResult<TSuccess>
    {
        Result OnSuccess(Func<TSuccess, Result> mapper);
        Result<TOut> OnSuccess<TOut>(Func<TSuccess, Result<TOut>> mapper);
        AsyncResult OnSuccess(Func<TSuccess, Task<Result>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut>> mapper);
        Result<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper);
        Result<TOut> OnSuccess<TOut>(Func<TSuccess, TOut> mapper); // <- Result.Success(TOut)
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, Task<TOut>> mapper); // <- Result.Success(TOut)


        Result OnFailure(Func<Result<TSuccess>, Result> mapper);
        Result<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Result<TOut>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, AsyncResult<TOut>> mapper);
        Result<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, AsyncResult<TOut, TFailure>> mapper);
        Result<TOut> OnFailure<TOut>(Func<Result<TSuccess>, TOut> mapper); // <- Result.Success(TOut)
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Task<TOut>> mapper); // <- Result.Success(TOut)
    }
}