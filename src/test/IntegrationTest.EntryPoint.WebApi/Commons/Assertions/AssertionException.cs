using System;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

[ExcludeFromCodeCoverage]
public sealed class AssertionException(string stackTrace, string message = null) : Exception(message)
{
    public override string StackTrace { get; } = stackTrace;

}