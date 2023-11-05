using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

public sealed class RegisterQueryStructureCommand
{
    private static readonly RegisterQueryStructureCommandValidator Validator = new RegisterQueryStructureCommandValidator();

    public string ClientUid { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public AiSettingsCommand AiSettings { get; init; }

    public RegisterQueryStructureCommand(string clientUid, string name, string description, AiSettingsCommand aiSettings)
    {
        ClientUid = clientUid;
        Name = name;
        Description = description;
        AiSettings = aiSettings;
        Validator.ValidateAndThrow(this);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(RegisterQueryStructureCommand other)
    {
        return ClientUid == other.ClientUid
               && Name == other.Name
               && Description == other.Description
               && AiSettings.Equals(other.AiSettings);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is RegisterQueryStructureCommand other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(ClientUid);
        hashCode.Add(Name);
        hashCode.Add(Description);
        hashCode.Add(AiSettings);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(ClientUid)}: {ClientUid} ")
            .Append($"{nameof(Name)}: {Name} ")
            .Append($"{nameof(Description)}: {Description} ")
            .Append($"{nameof(AiSettings)}: {AiSettings} ")
            .ToString();
    }
}