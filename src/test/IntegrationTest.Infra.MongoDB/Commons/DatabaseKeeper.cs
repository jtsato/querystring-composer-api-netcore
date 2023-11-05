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
    private readonly string _dummyCollectionName;
    private readonly string _dummySequenceCollectionName;
    private readonly string _clientCollectionName;
    private readonly string _clientSequenceCollectionName;
    private readonly string _queryStructureCollectionName;
    private readonly string _queryStructureSequenceCollectionName;

    public DatabaseKeeper(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory(configuration["MONGODB_URL"]);
        _databaseName = configuration["MONGODB_DATABASE"];
        _dummyCollectionName = configuration["DUMMY_COLLECTION_NAME"];
        _dummySequenceCollectionName = configuration["DUMMY_SEQUENCE_COLLECTION_NAME"];
        _clientCollectionName = configuration["CLIENT_COLLECTION_NAME"];
        _clientSequenceCollectionName = configuration["CLIENT_SEQUENCE_COLLECTION_NAME"];
        _queryStructureCollectionName = configuration["QUERY_STRUCTURE_COLLECTION_NAME"];
        _queryStructureSequenceCollectionName = configuration["QUERY_STRUCTURE_SEQUENCE_COLLECTION_NAME"];

        ClearCollectionsData();
    }

    public void ClearCollectionsData()
    {
        List<Task> tasks = new List<Task>
        {
            ClearCollectionsData(_dummyCollectionName, _dummySequenceCollectionName),
            ClearCollectionsData(_clientCollectionName, _clientSequenceCollectionName),
            ClearCollectionsData(_queryStructureCollectionName, _queryStructureSequenceCollectionName),
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