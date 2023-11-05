using System;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Commons.Models;

public sealed class Field
{
    [SwaggerSchema(Nullable = false, Description = "Attribute name")]
    public string Name { get; }

    [SwaggerSchema(Nullable = false, Description = "Error message description")]
    public string Message { get; }

    [SwaggerSchema(Nullable = true, Description = "Submitted value")]
    public string Value { get; }

    public Field(string name, string message, string value)
    {
        Name = name;
        Message = message;
        Value = value;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(Field other)
    {
        return Name == other.Name && Message == other.Message && Value == other.Value;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Field other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Message, Value);
    }
}