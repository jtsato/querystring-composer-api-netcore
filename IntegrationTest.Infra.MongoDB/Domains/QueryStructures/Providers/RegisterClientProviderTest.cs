using System;
using System.Threading.Tasks;
using Core.Domains.Clients.Gateways;
using Core.Domains.Clients.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

[Collection("Database collection")]
public sealed class RegisterClientProviderTest : IClassFixture<RegisterClientProviderTestFixture>
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IRegisterClientGateway _registerClientGateway;

    public RegisterClientProviderTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _registerClientGateway = context.ServiceResolver.Resolve<IRegisterClientGateway>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Fail to register client when client already exists")]
    public async Task FailToRegisterClientWhenClientAlreadyExists()
    {
        // Arrange
        // Act
        Exception exception = await Record.ExceptionAsync(() => _registerClientGateway.ExecuteAsync
            (
                new Client
                {
                    Uid = "490f1db4-ed14-4cdc-a09f-401048951b17",
                    Name = "already-exists-client-structure",
                    Description = "Already exists client",
                    CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                    UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local)
                }
            )
        );

        // Assert
        Assert.NotNull(exception);

        _outputHelper.WriteLine(exception.ToString());

        Assert.IsType<MongoWriteException>(exception);
        Assert.Contains("""E11000 duplicate key error collection: querystring-mongodb.query_structures index: uid_1 dup key: { uid: "490f1db4-ed14-4cdc-a09f-401048951b17" }""", exception.Message);
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to register client when client not exists")]
    public async Task SuccessfulToRegisterClientWhenClientNotExists()
    {
        // Arrange
        // Act
        Client client = await _registerClientGateway.ExecuteAsync
        (
            new Client
            {
                Uid = "490f1db4-ed14-4cdc-a09f-401048951b16",
                Name = "properties-searching-using-structure",
                Description = "Searching using structured queries",
                CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local)
            }
        );

        // Assert
        Assert.NotNull(client);

        _outputHelper.WriteLine(client.ToString());

        Assert.Equal("490f1db4-ed14-4cdc-a09f-401048951b16", client.Uid);
        Assert.Equal("properties-searching-using-structure", client.Name);
        Assert.Equal("Searching using structured queries", client.Description);
        Assert.Equal(new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local), client.CreatedAt);
        Assert.Equal(new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local), client.UpdatedAt);
    }
}