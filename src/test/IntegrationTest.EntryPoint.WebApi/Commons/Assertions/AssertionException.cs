using System;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

[ExcludeFromCodeCoverage]
public sealed class AssertionException : Exception
{
    public override string StackTrace { get; }

    public AssertionException(string stackTrace, string message = null) : base(message)
    {
        StackTrace = stackTrace;
    }
}