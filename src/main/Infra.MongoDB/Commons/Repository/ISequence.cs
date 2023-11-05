namespace Infra.MongoDB.Commons.Repository;

public interface ISequence
{
    string Id { get; init; }
    string SequenceName { get; init; }
    int SequenceValue { get; init; }
}