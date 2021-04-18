using System;
using System.Collections.Generic;

namespace Ergo
{
    internal interface IResult
    {
        IReadOnlyList<string> Messages { get; }
        bool IsSuccessful { get; }
        bool IsFailure { get; }
    }
}
