using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Infra.HttpClient.Domains.QueryStrings.Models;

public sealed class CompletionRequest
{
    [JsonPropertyName("model")] 
    public string Model { get; init; }

    [JsonPropertyName("temperature")] 
    public double Temperature { get; init; }

    [JsonPropertyName("max_tokens")] 
    public int MaxTokens { get; init; }

    [JsonPropertyName("prompt")] 
    public string Prompt { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(CompletionRequest other)
    {
        return Model == other.Model
               && Temperature.Equals(other.Temperature)
               && MaxTokens == other.MaxTokens
               && Prompt == other.Prompt;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is CompletionRequest other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Model);
        hashCode.Add(Temperature);
        hashCode.Add(MaxTokens);
        hashCode.Add(Prompt);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"Model: {Model}")
            .AppendLine($"Temperature: {Temperature}")
            .AppendLine($"MaxTokens: {MaxTokens}")
            .AppendLine($"Prompt: {Prompt}")
            .ToString();
    }
}
