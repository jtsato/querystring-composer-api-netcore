using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class AccessDeniedException : CoreException
{
    public AccessDeniedException(string message, params object[] args) : base(message, args)
    {
    }
}
