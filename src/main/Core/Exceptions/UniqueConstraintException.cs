using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class UniqueConstraintException(string message, params object[] args) : CoreException(message, args);
