using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Core.Commons;
using Core.Commons.Models;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public sealed class InvalidArgumentException : CoreException, ISerializable
{
    public IList<FieldError> FieldErrors { get; }

    public InvalidArgumentException(string message, IList<FieldError> fieldErrors, params object[] args) : base(message, args)
    {
        FieldErrors = fieldErrors;
    }

    private InvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ArgumentValidator.CheckNull(info, nameof(info));
        Parameters = (object[]) info.GetValue(nameof(Parameters), typeof(object[]));
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ArgumentValidator.CheckNull(info, nameof(info));
        info.AddValue(nameof(Parameters), Parameters, typeof(object[]));
    }
}