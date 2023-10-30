using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Commons.Models;

public sealed class ResponseStatus
{
    [SwaggerSchema(Nullable = false, Description = "HTTP response status code")]
    public int Code { get; }

    [SwaggerSchema(Nullable = false, Description = "Response error message")]
    public string Message { get; }

    [SwaggerSchema(Nullable = false, Description = "Detailed errors by field")]
    public List<Field> Fields { get; }

    public ResponseStatus(int code, string message)
    {
        Code = code;
        Message = message;
        Fields = new List<Field>(0);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(ResponseStatus other)
    {
        return Code == other.Code &&
               Message == other.Message
               && Fields.Count == other.Fields.Count
               && !Fields.Except(other.Fields).Any() && !other.Fields.Except(Fields).Any();
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is ResponseStatus other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message, Fields);
    }
}