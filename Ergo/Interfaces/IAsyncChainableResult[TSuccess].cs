using System;
using System.Threading.Tasks;

namespace Ergo
{
    public interface IAsyncChainableResult<TSuccess>
    {
        AsyncResult OnSuccess(Func<TSuccess, Result> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, Result<TOut>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut, TFailure>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, TOut> mapper); // <- Result.Success(TOut)
        AsyncResult<TOut> OnSuccess<TOut>(Func<TSuccess, Task<TOut>> mapper); // <- Result.Success(TOut)


        AsyncResult OnFailure(Func<Result<TSuccess>, Result> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Result<TOut>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, AsyncResult<TOut>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut, TFailure>(Func<Result<TSuccess>, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, TOut> mapper); // <- Result.Success(TOut)
        AsyncResult<TOut> OnFailure<TOut>(Func<Result<TSuccess>, Task<TOut>> mapper); // <- Result.Success(TOut)
    }
}
