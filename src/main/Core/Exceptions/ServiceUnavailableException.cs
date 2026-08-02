using System.Diagnostics.CodeAnalysis;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ServiceUnavailableException(string message, params object[] args) : CoreException(message, args);
