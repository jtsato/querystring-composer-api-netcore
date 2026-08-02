using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

public sealed class QueryStructureRepository : Repository<QueryStructureEntity>, IIndexInitializer
{
    public QueryStructureRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        : base(connectionFactory, databaseName, collectionName)
    {
    }

    public async Task EnsureIndexesAsync()
    {
        IndexKeysDefinition<QueryStructureEntity> indexKeyId = Builders<QueryStructureEntity>
            .IndexKeys.Ascending(document => document.Id);

        IndexKeysDefinition<QueryStructureEntity> indexKeyClientUid = Builders<QueryStructureEntity>
            .IndexKeys.Ascending(document => document.ClientUid);

        IndexKeysDefinition<QueryStructureEntity> indexKeyName = Builders<QueryStructureEntity>
            .IndexKeys.Ascending(document => document.Name);

        IndexKeysDefinition<QueryStructureEntity> indexKeyNameAndClientUid = Builders<QueryStructureEntity>
            .IndexKeys.Combine(indexKeyClientUid, indexKeyName);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        CreateIndexOptions nonUniqueIndexOptions = new CreateIndexOptions
            {Unique = false, Sparse = true, Background = false};

        await GetCollection().Indexes
            .CreateManyAsync([
                new CreateIndexModel<QueryStructureEntity>(indexKeyId, uniqueIndexOptions),
                new CreateIndexModel<QueryStructureEntity>(indexKeyClientUid, nonUniqueIndexOptions),
                new CreateIndexModel<QueryStructureEntity>(indexKeyNameAndClientUid, uniqueIndexOptions)
            ]);
    }
}
