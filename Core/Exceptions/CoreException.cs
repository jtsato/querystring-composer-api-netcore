using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Core.Commons;

namespace Core.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class CoreException : Exception, ISerializable
{
    public object[] Parameters { get; set; }

    protected CoreException(string message, params object[] args) : base(message)
    {
        Parameters = args;
    }

    protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
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