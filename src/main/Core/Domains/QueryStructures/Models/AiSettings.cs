using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.QueryStructures.Models;

public sealed class AiSettings
{
    public int UsagePercentage { get; init; }
    public string ApiKey { get; init; }
    public string Model { get; init; }
    public double Temperature { get; init; }
    public int MaxTokens { get; init; }
    public string PromptTemplate { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(AiSettings other)
    {
        return UsagePercentage.Equals(other.UsagePercentage)
               && ApiKey == other.ApiKey
               && Model == other.Model
               && Temperature.Equals(other.Temperature)
               && MaxTokens == other.MaxTokens
               && PromptTemplate == other.PromptTemplate;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is AiSettings other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(UsagePercentage);
        hashCode.Add(ApiKey);
        hashCode.Add(Model);
        hashCode.Add(Temperature);
        hashCode.Add(MaxTokens);
        hashCode.Add(PromptTemplate);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(UsagePercentage)}: {UsagePercentage} ")
            .Append($"{nameof(ApiKey)}: {ApiKey} ")
            .Append($"{nameof(Model)}: {Model} ")
            .Append($"{nameof(Temperature)}: {Temperature} ")
            .Append($"{nameof(MaxTokens)}: {MaxTokens} ")
            .Append($"{nameof(PromptTemplate)}: {PromptTemplate} ")
            .ToString();
    }
}