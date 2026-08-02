using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ServiceUnavailableException : CoreException
{
    public ServiceUnavailableException(string message, params object[] args) : base(message, args)
    {
    }
}
