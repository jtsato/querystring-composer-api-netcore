using Core.Domains.QueryStructures.Interfaces;
using IntegrationTest.Infra.MongoDB.Commons;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

[Collection("Database collection")]
public sealed class UpdateQueryStructureProviderTest(ITestOutputHelper outputHelper, Context context) : IClassFixture<UpdateQueryStructureProviderTestFixture>
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;
    private readonly IUpdateQueryStructureGateway _updateQueryStructureGateway = context.ServiceResolver.Resolve<IUpdateQueryStructureGateway>();

    // [Trait("Category", "Infrastructure (DB) Integration tests")]
    // [Fact(DisplayName = "Fail to update query structure when query structure does not exist")]
    // public async Task FailToUpdateQueryStructureWhenQueryStructureDoesNotExist()
}