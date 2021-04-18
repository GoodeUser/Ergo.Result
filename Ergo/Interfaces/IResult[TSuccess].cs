namespace Ergo
{
    internal interface IResult<TSuccess> : IResult
    {
        TSuccess GetSuccessValue();
        TSuccess GetSuccessValueOrDefault(TSuccess defaultValue);
    }
}