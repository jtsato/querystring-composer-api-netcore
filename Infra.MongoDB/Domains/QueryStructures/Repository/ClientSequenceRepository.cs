using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

[ExcludeFromCodeCoverage]
public sealed class ClientSequenceRepository : ISequenceRepository<ClientSequence>
{
    private readonly IMongoCollection<ClientSequence> _collection;
    
    public ClientSequenceRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
    {
        IMongoDatabase database = connectionFactory.GetDatabase(databaseName);
        _collection = database.GetCollection<ClientSequence>(collectionName);

        IndexKeysDefinition<ClientSequence> indexKeySequenceName = Builders<ClientSequence>
            .IndexKeys.Ascending(sequence => sequence.SequenceName);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        _collection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<ClientSequence>(indexKeySequenceName, uniqueIndexOptions)
        });
    }
    
    public async Task<ISequence> GetSequenceAndUpdate(FilterDefinition<ClientSequence> filterDefinition)
    {
        FindOneAndUpdateOptions<ClientSequence> findOneAndUpdateOptions =
            new FindOneAndUpdateOptions<ClientSequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
        
        UpdateDefinition<ClientSequence> updateDefinition = Builders<ClientSequence>.Update.Inc(sequence => sequence.SequenceValue, 1);
        
        return await _collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, findOneAndUpdateOptions);
    }
}
