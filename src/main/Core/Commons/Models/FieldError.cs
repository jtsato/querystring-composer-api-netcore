using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Commons.Models;

public sealed class FieldError
{
    public string PropertyName { get; init; }

    public string ErrorMessage { get; init; }

    public string AttemptedValue { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(FieldError other)
    {
        return PropertyName == other.PropertyName
               && ErrorMessage == other.ErrorMessage
               && AttemptedValue == other.AttemptedValue;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is FieldError other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(PropertyName, ErrorMessage, AttemptedValue);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(PropertyName)}: {PropertyName} ")
            .Append($"{nameof(ErrorMessage)}: {ErrorMessage} ")
            .Append($"{nameof(AttemptedValue)}: {AttemptedValue} ")
            .ToString();
    }
}