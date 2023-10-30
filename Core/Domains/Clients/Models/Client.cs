using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Clients.Models;

public sealed class Client
{
    public int Id { get; init; }
    public string Uid { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(Client other)
    {
        return Id == other.Id
               && Uid == other.Uid
               && Name == other.Name
               && Description == other.Description
               && CreatedAt.Equals(other.CreatedAt)
               && UpdatedAt.Equals(other.UpdatedAt);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Client other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(Name);
        hashCode.Add(Description);
        hashCode.Add(Uid);
        hashCode.Add(CreatedAt);
        hashCode.Add(UpdatedAt);
        
        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Id)}: {Id} ")
            .Append($"{nameof(Uid)}: {Uid} ")
            .Append($"{nameof(Name)}: {Name} ")
            .Append($"{nameof(Description)}: {Description} ")
            .Append($"{nameof(CreatedAt)}: {CreatedAt} ")
            .Append($"{nameof(UpdatedAt)}: {UpdatedAt} ")
            .ToString();
    }
}
