using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class QueryStructureEntity : Entity
{
    [BsonElement("clientUid")]
    public string ClientUid { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
    [BsonElement("aiSettings")]
    public AiSettingsEntity AiSettings { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
    
    [BsonElement("items")]
    public IList<ItemEntity> Items { get; set; }
}
