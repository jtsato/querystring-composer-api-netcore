using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Repository;

public sealed class ClientRepository : Repository<ClientEntity>, IIndexInitializer
{
    public ClientRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        : base(connectionFactory, databaseName, collectionName)
    {
    }

    public async Task EnsureIndexesAsync()
    {
        IndexKeysDefinition<ClientEntity> indexKeyId = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Id);

        IndexKeysDefinition<ClientEntity> indexKeyUid = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Uid);

        IndexKeysDefinition<ClientEntity> indexKeyName = Builders<ClientEntity>
            .IndexKeys.Ascending(document => document.Name);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        await GetCollection().Indexes
            .CreateManyAsync([
                new CreateIndexModel<ClientEntity>(indexKeyId, uniqueIndexOptions),
                new CreateIndexModel<ClientEntity>(indexKeyUid, uniqueIndexOptions),
                new CreateIndexModel<ClientEntity>(indexKeyName, uniqueIndexOptions)
            ]);
    }
}
