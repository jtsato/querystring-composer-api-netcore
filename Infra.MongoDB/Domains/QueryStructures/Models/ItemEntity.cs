using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class ItemEntity {
    
    [BsonElement("rank")]
    public int Rank { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
    [BsonElement("isCountable")]
    public bool IsCountable { get; set; }
    
    [BsonElement("waitForConfirmationWords")]
    public bool WaitForConfirmationWords { get; set; }
    
    [BsonElement("entries")]
    public IList<EntryEntity> Entries { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonElement("confirmationWords")]
    public IList<string> ConfirmationWords { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonElement("revocationWords")]
    public IList<string> RevocationWords { get; set; }
}
