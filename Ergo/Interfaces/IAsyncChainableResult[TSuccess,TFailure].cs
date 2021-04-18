using System;

namespace Ergo
{
    public interface IAsyncChainableResult<TSuccess,TFailure>
    {
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TOut> mapper);
        AsyncResult<TSuccess, TFailure> OnSuccess(Func<TSuccess, TFailure> mapper);

        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TOut> mapper);
        AsyncResult<TSuccess, TFailure> OnFailure(Func<Result<TSuccess, TFailure>, TFailure> mapper);
    }
}