namespace Ergo
{
    internal interface IResult<TSuccess,TFailure> : IResult<TSuccess>
    {
        TFailure GetFailureValue();
        TFailure GetFailureValueOrDefault(TFailure defaultValue);
    }
}