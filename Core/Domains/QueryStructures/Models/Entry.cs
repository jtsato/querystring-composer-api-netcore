using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Core.Domains.QueryStructures.Models;

public sealed class Entry
{
    public int Rank { get; init; }
    public string Key { get; init; }
    // It removes the other entries from the list when it is the best ranked.
    public bool Exclusive { get; init; }
    // It removes itself from the list when it is not the best ranked.
    public bool Immiscible { get; init; }
    
    public IList<string> KeyWords { get; init; } = new List<string>();
    public IDictionary<string, string> IncompatibleWith { get; init; } = new Dictionary<string, string>();

    [ExcludeFromCodeCoverage]
    private bool Equals(Entry other)
    {
        return Rank == other.Rank
               && Key == other.Key
               && Exclusive == other.Exclusive
               && Immiscible == other.Immiscible
               && KeyWords.SequenceEqual(other.KeyWords)
               && IncompatibleWith.SequenceEqual(other.IncompatibleWith);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Entry other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Rank);
        hashCode.Add(Key);
        hashCode.Add(Exclusive);
        hashCode.Add(Immiscible);
        hashCode.Add(KeyWords);
        hashCode.Add(IncompatibleWith);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Rank)}: {Rank} ")
            .Append($"{nameof(Key)}: {Key} ")
            .Append($"{nameof(Exclusive)}: {Exclusive} ")
            .Append($"{nameof(Immiscible)}: {Immiscible} ")
            .Append($"{nameof(KeyWords)}: {string.Join(", ", KeyWords)} ")
            .Append($"{nameof(IncompatibleWith)}: {string.Join(", ", IncompatibleWith)} ")
            .ToString();
    }
}