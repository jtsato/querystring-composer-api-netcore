using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Core.Domains.QueryStructures.Models;

public sealed class QueryStructure
{
    public int Id { get; init; }
    public string ClientUid { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public AiSettings AiSettings { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public IList<Item> Items { get; init; } = new List<Item>();

    [ExcludeFromCodeCoverage]
    private bool Equals(QueryStructure other)
    {
        return Id == other.Id
               && ClientUid == other.ClientUid
               && Name == other.Name
               && Description == other.Description
               && AiSettings.Equals(other.AiSettings)
               && CreatedAt.Equals(other.CreatedAt)
               && UpdatedAt.Equals(other.UpdatedAt)
               && Items.SequenceEqual(other.Items);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is QueryStructure other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(ClientUid);
        hashCode.Add(Name);
        hashCode.Add(Description);
        hashCode.Add(AiSettings);
        hashCode.Add(CreatedAt);
        hashCode.Add(UpdatedAt);
        hashCode.Add(Items);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Id)}: {Id} ")
            .Append($"{nameof(Name)}: {Name} ")
            .Append($"{nameof(Description)}: {Description} ")
            .Append($"{nameof(AiSettings)}: {AiSettings} ")
            .Append($"{nameof(CreatedAt)}: {CreatedAt} ")
            .Append($"{nameof(UpdatedAt)}: {UpdatedAt} ")
            .Append($"{nameof(Items)}: {string.Join(", ", Items)} ")
            .ToString();
    }
}