using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Connection;

public interface IConnectionFactory
{
    IMongoClient GetClient();

    IMongoDatabase GetDatabase(string databaseName);
}