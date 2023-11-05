using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Commons.Repository;

[BsonIgnoreExtraElements]
public abstract class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string ObjectId { get; set; }

    [BsonElement("id")]
    public int Id { get; set; }
}