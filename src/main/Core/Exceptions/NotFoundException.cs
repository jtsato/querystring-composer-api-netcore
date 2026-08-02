using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class NotFoundException : CoreException
{
    public NotFoundException(string message, params object[] args) : base(message, args)
    {
    }
}
