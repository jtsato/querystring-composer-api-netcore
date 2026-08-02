using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Connection;

public sealed class ConnectionFactory(string connectionString) : IConnectionFactory
{

    public IMongoClient GetClient()
    {
        return new MongoClient(connectionString);
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        IMongoClient mongoDbClient = GetClient();

        return mongoDbClient.GetDatabase(databaseName);
    }
}