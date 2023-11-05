using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class EntryEntity
{
    [BsonElement("rank")]
    public int Rank { get; set; }
    
    [BsonElement("key")]
    public string Key { get; set; }
    
    [BsonElement("exclusive")]
    public bool Exclusive { get; set; }
    
    [BsonElement("immiscible")]
    public bool Immiscible { get; set; }
    
    [BsonElement("KeyWords")]
    public IList<string> KeyWords { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonElement("IncompatibleWith")]
    public IDictionary<string, string> IncompatibleWith { get; set; }
}
