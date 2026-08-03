using System;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Connection;

public sealed class ConnectionFactory(string connectionString) : IConnectionFactory
{
    private readonly Lazy<IMongoClient> _client = new(() => CreateClient(connectionString));

    public IMongoClient GetClient()
    {
        return _client.Value;
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        return GetClient().GetDatabase(databaseName);
    }

    private static MongoClient CreateClient(string connectionString)
    {
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

        return new MongoClient(settings);
    }
}
