using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons.Models;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class InvalidArgumentException(string message, IList<FieldError> fieldErrors, params object[] args) : CoreException(message, args)
{
    public IList<FieldError> FieldErrors { get; } = fieldErrors;

}
