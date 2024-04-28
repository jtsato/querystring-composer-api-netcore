using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.QueryStructures.Models;

// ReSharper disable ClassNeverInstantiated.Global
[ExcludeFromCodeCoverage]
public sealed class ClientSequence : ISequence
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; init; }

    [BsonElement("sequence_name")]
    public string SequenceName { get; init; }
    
    [BsonElement("sequence_value")]
    public int SequenceValue { get; init; }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(SequenceName)}: {SequenceName}, " +
               $"{nameof(SequenceValue)}: {SequenceValue}";
    }
}