using System;

namespace Ergo
{
    /// <summary>
    /// Define the methods that we allow a Result object to have once it has a
    /// `TFailure` type. We restrain the type output because once a type has a
    /// failure result (or status code), we want to make sure that we pass that
    /// failure all the way through.
    /// </summary>
    /// <typeparam name="TSuccess"></typeparam>
    /// <typeparam name="TFailure"></typeparam>
    public interface IChainableResult<TSuccess,TFailure>
    {
        Result<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TOut> mapper);
        AsyncResult<TOut, TFailure> OnSuccess<TOut>(Func<TSuccess, TFailure> mapper);

        Result<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, Result<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, AsyncResult<TOut, TFailure>> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TOut> mapper);
        AsyncResult<TOut, TFailure> OnFailure<TOut>(Func<Result<TSuccess, TFailure>, TFailure> mapper);
    }
}