using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ParentConstraintException : CoreException
{
    public ParentConstraintException(string message, params object[] args) : base(message, args)
    {
    }
}
