using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class AiSettingsEntity
{
    [BsonElement("usagePercentage")]
    public int UsagePercentage { get; set; }
    
    [BsonElement("apiKey")]
    public string ApiKey { get; set; }
    
    [BsonElement("model")]
    public string Model { get; set; }
    
    [BsonElement("temperature")]
    public double Temperature { get; set; }
    
    [BsonElement("maxTokens")]
    public int MaxTokens { get; set; }
    
    [BsonElement("promptTemplate")]
    public string PromptTemplate { get; set; }
}
