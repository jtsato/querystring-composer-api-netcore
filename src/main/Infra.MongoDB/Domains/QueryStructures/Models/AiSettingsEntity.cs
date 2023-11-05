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
    public byte UsagePercentage { get; set; }
    
    [BsonElement("apiKey")]
    public string ApiKey { get; set; }
    
    [BsonElement("model")]
    public string Model { get; set; }
    
    [BsonElement("temperature")]
    public float Temperature { get; set; }
    
    [BsonElement("maxTokens")]
    public int MaxTokens { get; set; }
    
    [BsonElement("promptTemplate")]
    public string PromptTemplate { get; set; }
}
