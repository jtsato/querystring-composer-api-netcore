using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

public sealed class ClientRepository : Repository<ClientEntity>
{
    public ClientRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        : base(connectionFactory, databaseName, collectionName)
    {
        IndexKeysDefinition<ClientEntity> indexKeyId = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Id);

        IndexKeysDefinition<ClientEntity> indexKeyUid = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Uid);

        IndexKeysDefinition<ClientEntity> indexKeyName = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Name);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        GetCollection().Indexes
            .CreateManyAsync(new[]
            {
                new CreateIndexModel<ClientEntity>(indexKeyId, uniqueIndexOptions),
                new CreateIndexModel<ClientEntity>(indexKeyUid, uniqueIndexOptions),
                new CreateIndexModel<ClientEntity>(indexKeyName, uniqueIndexOptions)
            });
    }
}