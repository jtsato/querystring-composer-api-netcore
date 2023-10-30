using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

public sealed class AiSettingsCommand
{
    private static readonly AiSettingsCommandValidator Validator = new AiSettingsCommandValidator();

    public string UsagePercentage { get; init; }
    public string ApiKey { get; init; }
    public string Model { get; init; }
    public string Temperature { get; init; }
    public string MaxTokens { get; init; }
    public string PromptTemplate { get; init; }
    
    public AiSettingsCommand(string usagePercentage, string apiKey, string model, string temperature, string maxTokens, string promptTemplate)
    {
        UsagePercentage = usagePercentage;
        ApiKey = apiKey;
        Model = model;
        Temperature = temperature;
        MaxTokens = maxTokens;
        PromptTemplate = promptTemplate;
        Validator.ValidateAndThrow(this);
    }
    
    [ExcludeFromCodeCoverage]
    private bool Equals(AiSettingsCommand other)
    {
        return UsagePercentage == other.UsagePercentage
               && ApiKey == other.ApiKey
               && Model == other.Model
               && Temperature.Equals(other.Temperature)
               && MaxTokens == other.MaxTokens
               && PromptTemplate == other.PromptTemplate;
    }
    
    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is AiSettingsCommand other && Equals(other);
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
