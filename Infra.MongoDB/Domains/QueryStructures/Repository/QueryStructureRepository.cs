using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

public sealed class QueryStructureRepository : Repository<QueryStructureEntity>
{
    public QueryStructureRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName) 
        : base(connectionFactory, databaseName, collectionName)
    {
        IndexKeysDefinition<QueryStructureEntity> indexKeyId = Builders<QueryStructureEntity>
            .IndexKeys.Ascending(document => document.Id);

        IndexKeysDefinition<QueryStructureEntity> indexKeyClientUid = Builders<QueryStructureEntity>
            .IndexKeys.Ascending(document => document.ClientUid);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};
        
        CreateIndexOptions nonUniqueIndexOptions = new CreateIndexOptions
            {Unique = false, Sparse = true, Background = false};

        GetCollection().Indexes
            .CreateManyAsync(new[]
            {
                new CreateIndexModel<QueryStructureEntity>(indexKeyId, uniqueIndexOptions),
                new CreateIndexModel<QueryStructureEntity>(indexKeyClientUid, nonUniqueIndexOptions)
            });
    }
}