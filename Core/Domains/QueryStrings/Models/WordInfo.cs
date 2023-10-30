using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.QueryStrings.Models;

public class WordInfo
{
    public WordInfoType Type { get; set; }
    public string Value { get; set; }

    [ExcludeFromCodeCoverage]
    private bool Equals(WordInfo other)
    {
        return Equals(Type, other.Type) && Value == other.Value;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is WordInfo other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Type);
        hashCode.Add(Value);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Type)}: {Type} ")
            .Append($"{nameof(Value)}: {Value} ")
            .ToString();
    }
}