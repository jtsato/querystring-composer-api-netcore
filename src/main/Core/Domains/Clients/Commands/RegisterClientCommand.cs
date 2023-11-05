using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.Clients.Commands;

public sealed class RegisterClientCommand
{
    private static readonly RegisterClientCommandValidator Validator = new RegisterClientCommandValidator();
    
    public string Uid { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    
    public RegisterClientCommand(string uid, string name, string description)
    {
        Uid = uid;
        Name = name;
        Description = description;
        Validator.ValidateAndThrow(this);
    }
    
    [ExcludeFromCodeCoverage]
    private bool Equals(RegisterClientCommand other)
    {
        return Uid == other.Uid
               && Name == other.Name
               && Description == other.Description;
    }
    
    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is RegisterClientCommand other && Equals(other);
    }
    
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Uid);
        hashCode.Add(Name);
        hashCode.Add(Description);
        
        return hashCode.ToHashCode();
    }
    
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Uid)}: {Uid} ")
            .Append($"{nameof(Name)}: {Name} ")
            .Append($"{nameof(Description)}: {Description} ")
            .ToString();
    }
}