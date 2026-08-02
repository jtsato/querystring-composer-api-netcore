using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public class CoreException : Exception
{
    public object[] Parameters { get; set; }

    protected CoreException(string message, params object[] args) : base(message)
    {
        Parameters = args;
    }
}
