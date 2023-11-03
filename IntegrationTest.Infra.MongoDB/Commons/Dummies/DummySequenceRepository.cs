using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Commons.Dummies;

[ExcludeFromCodeCoverage]
public sealed class DummySequenceRepository : ISequenceRepository<DummySequence>
{
    private readonly IMongoCollection<DummySequence> _collection;
    
    public DummySequenceRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
    {
        IMongoDatabase database = connectionFactory.GetDatabase(databaseName);
        _collection = database.GetCollection<DummySequence>(collectionName);

        IndexKeysDefinition<DummySequence> indexKeySequenceName = Builders<DummySequence>
            .IndexKeys.Ascending(sequence => sequence.SequenceName);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        _collection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<DummySequence>(indexKeySequenceName, uniqueIndexOptions)
        });
    }
    
    public async Task<ISequence> GetSequenceAndUpdate(FilterDefinition<DummySequence> filterDefinition)
    {
        FindOneAndUpdateOptions<DummySequence> findOneAndUpdateOptions =
            new FindOneAndUpdateOptions<DummySequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
        
        UpdateDefinition<DummySequence> updateDefinition = Builders<DummySequence>.Update.Inc(sequence => sequence.SequenceValue, 1);
        
        return await _collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, findOneAndUpdateOptions);
    }
}