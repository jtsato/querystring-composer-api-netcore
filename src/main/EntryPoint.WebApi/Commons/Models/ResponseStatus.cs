using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Commons.Models;

public sealed class ResponseStatus(int code, string message)
{
    [SwaggerSchema(Nullable = false, Description = "HTTP response status code")]
    public int Code { get; } = code;

    [SwaggerSchema(Nullable = false, Description = "Response error message")]
    public string Message { get; } = message;

    [SwaggerSchema(Nullable = false, Description = "Detailed errors by field")]
    public List<Field> Fields { get; } = [];

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