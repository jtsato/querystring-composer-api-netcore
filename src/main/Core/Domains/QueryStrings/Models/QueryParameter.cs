using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Domains.QueryStrings.Models;

// One resolved query string parameter, for example types=APARTMENT,HOUSE.
//
// It replaces the "name=value" strings the builder used to pass around, which had to be split back
// apart with Split('=') every time the name or the values were needed again.
public sealed class QueryParameter
{
    public string Name { get; }
    public IReadOnlyList<string> Values { get; }

    public QueryParameter(string name, IEnumerable<string> values)
    {
        Name = name;
        Values = values.ToList();
    }

    public string ValuesAsText => string.Join(",", Values);

    public override string ToString()
    {
        return $"{Name}={ValuesAsText}";
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(QueryParameter other)
    {
        return Name == other.Name && Values.SequenceEqual(other.Values);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is QueryParameter other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Name);
        hashCode.Add(Values);

        return hashCode.ToHashCode();
    }
}
