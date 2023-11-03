using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Extensions;
using Infra.MongoDB.Commons.Repository;
using IntegrationTest.Infra.MongoDB.Commons.Dummies;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Commons.Extensions;

[Collection("Database collection")]
public sealed class QueryByPageExtensionsTest : IClassFixture<QueryByPageExtensionsTestFixture>
{
    private readonly ITestOutputHelper _outputHelper;
    readonly IRepository<DummyEntity> _dummyRepository;

    public QueryByPageExtensionsTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _dummyRepository = context.ServiceResolver.Resolve<IRepository<DummyEntity>>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to query by page")]
    public async Task SuccessfulToQueryByPage()
    {
        // Arrange
        IMongoCollection<DummyEntity> collection = _dummyRepository.GetCollection();

        // Act
        (int totalPages, long totalOfElements, IReadOnlyList<DummyEntity> content) result = await collection.AggregateByPage
        (
            filterDefinition: Builders<DummyEntity>.Filter.Empty,
            sortDefinition: Builders<DummyEntity>.Sort.Ascending("Name"),
            pageNumber: 1,
            pageSize: 10
        );

        _outputHelper.WriteLine(result.ToString());

        // Assert
        Assert.True(result.totalPages >= 0);
        Assert.True(result.totalOfElements >= 0);
        Assert.NotNull(result.content);
    }
}