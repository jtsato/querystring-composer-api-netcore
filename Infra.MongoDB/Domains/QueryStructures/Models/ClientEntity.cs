using System;
using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class ClientEntity : Entity
{
    [BsonElement("uid")]
    public string Uid { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; init; }
    
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; init; }
}
