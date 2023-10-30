using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Commons;

public sealed class DatabaseKeeper
{
    private readonly IConnectionFactory _connectionFactory;

    private readonly string _databaseName;
    private readonly string _propertyCollectionName;
    private readonly string _propertySequenceCollectionName;

    public DatabaseKeeper(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory(configuration["MONGODB_URL"]);
        _databaseName = configuration["MONGODB_DATABASE"];
        _propertyCollectionName = configuration["QUERY_STRUCTURE_COLLECTION_NAME"];
        _propertySequenceCollectionName = configuration["QUERY_STRUCTURE_SEQUENCE_COLLECTION_NAME"];

        ClearCollectionsData();
    }

    public void ClearCollectionsData()
    {
        List<Task> tasks = new List<Task>
        {
            ClearCollectionsData(_propertyCollectionName, _propertySequenceCollectionName),
        };

        Task.WhenAll(tasks);
    }

    private async Task ClearCollectionsData(string collectionName, string sequenceCollectionName)
    {
        await ClearCollectionsData(collectionName);
        await ClearCollectionsData(sequenceCollectionName);
    }

    private async Task ClearCollectionsData(string collectionName)
    {
        await _connectionFactory
            .GetDatabase(_databaseName)
            .GetCollection<BsonDocument>(collectionName)
            .DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
    }
}