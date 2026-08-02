using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class NotFoundException(string message, params object[] args) : CoreException(message, args);
