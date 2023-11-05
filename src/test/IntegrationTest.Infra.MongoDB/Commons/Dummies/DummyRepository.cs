using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Commons.Dummies;

public sealed class DummyRepository : Repository<DummyEntity>
{
    public DummyRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        : base(connectionFactory, databaseName, collectionName)
    {
        IndexKeysDefinition<DummyEntity> indexKeyId = Builders<DummyEntity>
            .IndexKeys.Ascending(document => document.Id);

        IndexKeysDefinition<DummyEntity> indexKeyName = Builders<DummyEntity>
            .IndexKeys.Ascending(document => document.Name);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        GetCollection().Indexes
            .CreateManyAsync(new[]
            {
                new CreateIndexModel<DummyEntity>(indexKeyId, uniqueIndexOptions),
                new CreateIndexModel<DummyEntity>(indexKeyName, uniqueIndexOptions)
            });
    }
}