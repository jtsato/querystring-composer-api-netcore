using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons.Models;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class InvalidArgumentException : CoreException
{
    public IList<FieldError> FieldErrors { get; }

    public InvalidArgumentException(string message, IList<FieldError> fieldErrors, params object[] args) : base(message, args)
    {
        FieldErrors = fieldErrors;
    }
}
