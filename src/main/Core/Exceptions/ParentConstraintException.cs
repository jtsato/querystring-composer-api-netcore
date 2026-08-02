using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ParentConstraintException(string message, params object[] args) : CoreException(message, args);
