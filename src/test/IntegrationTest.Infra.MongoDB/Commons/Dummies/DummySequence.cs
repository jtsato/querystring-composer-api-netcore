using Infra.MongoDB.Commons.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationTest.Infra.MongoDB.Commons.Dummies;

public sealed class DummySequence : ISequence
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; init; }

    public string SequenceName { get; init; }
    public int SequenceValue { get; init; }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(SequenceName)}: {SequenceName}, " +
               $"{nameof(SequenceValue)}: {SequenceValue}";
    }
}