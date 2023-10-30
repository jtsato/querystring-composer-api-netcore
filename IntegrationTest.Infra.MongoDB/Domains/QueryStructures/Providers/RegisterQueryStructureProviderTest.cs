using System;
using System.Threading.Tasks;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

[Collection("Database collection")]
public sealed class RegisterQueryStructureProviderTest : IClassFixture<RegisterQueryStructureProviderTestFixture>
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IRegisterQueryStructureGateway _registerQueryStructureGateway;

    public RegisterQueryStructureProviderTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _registerQueryStructureGateway = context.ServiceResolver.Resolve<IRegisterQueryStructureGateway>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Fail to register query structure when query structure already exists")]
    public async Task FailToRegisterQueryStructureWhenQueryStructureAlreadyExists()
    {
        // Arrange
        // Act
        Exception exception = await Record.ExceptionAsync(() => _registerQueryStructureGateway.ExecuteAsync
            (
                new QueryStructure
                {
                    ClientUid = "490f1db4-ed14-4cdc-a09f-401048951b17",
                    Name = "already-exists-query-structure",
                    Description = "Already exists query structure",
                    AiSettings = new AiSettings
                    {
                        UsagePercentage = 50,
                        ApiKey = "api-key",
                        Model = "model",
                        Temperature = 0.2f,
                        MaxTokens = 100,
                        PromptTemplate = "prompt-template"
                    },
                    CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                    UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local)
                }
            )
        );

        // Assert
        Assert.NotNull(exception);

        _outputHelper.WriteLine(exception.ToString());

        Assert.IsType<MongoWriteException>(exception);
        Assert.Contains("""Category : "DuplicateKey""", exception.Message);
        Assert.Contains("""E11000 duplicate key error collection: querystring-mongodb.query_structures index: name_1 dup key: { name: "already-exists-query-structure" }""", exception.Message);
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to register query structure when query structure not exists")]
    public async Task SuccessfulToRegisterQueryStructureWhenQueryStructureNotExists()
    {
        // Arrange
        // Act
        QueryStructure queryStructure = await _registerQueryStructureGateway.ExecuteAsync
        (
            new QueryStructure
            {
                ClientUid = "490f1db4-ed14-4cdc-a09f-401048951b15",
                Name = "properties-searching-using-structure",
                Description = "Searching using structured queries",
                AiSettings = new AiSettings
                {
                    UsagePercentage = 50,
                    ApiKey = "api-key",
                    Model = "model",
                    Temperature = 0.2f,
                    MaxTokens = 100,
                    PromptTemplate = "prompt-template"
                },
                CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local)
            }
        );

        // Assert
        Assert.NotNull(queryStructure);

        _outputHelper.WriteLine(queryStructure.ToString());

        Assert.Equal("490f1db4-ed14-4cdc-a09f-401048951b15", queryStructure.ClientUid);
        Assert.Equal("properties-searching-using-structure", queryStructure.Name);
        Assert.Equal("Searching using structured queries", queryStructure.Description);
        Assert.Equal(50, queryStructure.AiSettings.UsagePercentage);
        Assert.Equal("api-key", queryStructure.AiSettings.ApiKey);
        Assert.Equal("model", queryStructure.AiSettings.Model);
        Assert.Equal(0.2f, queryStructure.AiSettings.Temperature);
        Assert.Equal(100, queryStructure.AiSettings.MaxTokens);
        Assert.Equal("prompt-template", queryStructure.AiSettings.PromptTemplate);
        Assert.Equal(new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local), queryStructure.CreatedAt);
        Assert.Equal(new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local), queryStructure.UpdatedAt);
    }
}
