using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

public class QueryStructureSequenceRepository : ISequenceRepository<QueryStructureSequence>
{
    private readonly IMongoCollection<QueryStructureSequence> _collection;

    public QueryStructureSequenceRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
    {
        IMongoDatabase database = connectionFactory.GetDatabase(databaseName);
        _collection = database.GetCollection<QueryStructureSequence>(collectionName);

        IndexKeysDefinition<QueryStructureSequence> indexKeySequenceName = Builders<QueryStructureSequence>
            .IndexKeys.Ascending(sequence => sequence.SequenceName);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        _collection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<QueryStructureSequence>(indexKeySequenceName, uniqueIndexOptions)
        });
    }

    public async Task<ISequence> GetSequenceAndUpdate(FilterDefinition<QueryStructureSequence> filterDefinition)
    {
        FindOneAndUpdateOptions<QueryStructureSequence> findOneAndUpdateOptions =
            new FindOneAndUpdateOptions<QueryStructureSequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

        UpdateDefinition<QueryStructureSequence> updateDefinition = Builders<QueryStructureSequence>.Update.Inc(sequence => sequence.SequenceValue, 1);

        return await _collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, findOneAndUpdateOptions);
    }
}
