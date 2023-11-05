using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Core.Domains.QueryStructures.Models;

public sealed class Item
{
    public int Rank { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsCountable { get; init; }
    public bool WaitForConfirmationWords { get; init; }

    public IList<Entry> Entries { get; init; } = new List<Entry>();
    public IList<string> ConfirmationWords { get; init; } = new List<string>();
    public IList<string> RevocationWords { get; init; } = new List<string>();

    [ExcludeFromCodeCoverage]
    private bool Equals(Item other)
    {
        return Rank == other.Rank
               && Name == other.Name
               && Description == other.Description
               && IsCountable == other.IsCountable
               && WaitForConfirmationWords == other.WaitForConfirmationWords
               && Entries.SequenceEqual(other.Entries)
               && ConfirmationWords.SequenceEqual(other.ConfirmationWords)
               && RevocationWords.SequenceEqual(other.RevocationWords);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Item other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Rank);
        hashCode.Add(Name);
        hashCode.Add(Description);
        hashCode.Add(IsCountable);
        hashCode.Add(WaitForConfirmationWords);
        hashCode.Add(ConfirmationWords);
        hashCode.Add(RevocationWords);
        hashCode.Add(Entries);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Rank)}: {Rank} ")
            .Append($"{nameof(Name)}: {Name} ")
            .Append($"{nameof(Description)}: {Description} ")
            .Append($"{nameof(IsCountable)}: {IsCountable} ")
            .Append($"{nameof(WaitForConfirmationWords)}: {WaitForConfirmationWords} ")
            .Append($"{nameof(Entries)}: {string.Join(", ", Entries)} ")
            .Append($"{nameof(ConfirmationWords)}: {string.Join(", ", ConfirmationWords)} ")
            .Append($"{nameof(RevocationWords)}: {string.Join(", ", RevocationWords)} ")
            .ToString();
    }
}