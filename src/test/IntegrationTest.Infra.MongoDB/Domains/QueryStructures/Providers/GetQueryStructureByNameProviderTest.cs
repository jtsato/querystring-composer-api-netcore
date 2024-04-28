using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Extensions;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

[Collection("Database collection")]
public sealed class GetQueryStructureByNameProviderTest : IClassFixture<GetQueryStructureByNameProviderTestFixture>
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IGetQueryStructureByNameGateway _getQueryStructureByNameGateway;

    public GetQueryStructureByNameProviderTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _getQueryStructureByNameGateway = context.ServiceResolver.Resolve<IGetQueryStructureByNameGateway>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to get query structure by client uid and name")]
    public async Task SuccessfulToGetQueryStructureByClientUidAndName()
    {
        // Arrange
        // Act
        Optional<QueryStructure> optional = await _getQueryStructureByNameGateway.ExecuteAsync
        (
            clientUid: "490f1db4-ed14-4cdc-a09f-401048951b15",
            name: "properties-search-query-structure"
        );

        // Assert
        Assert.True(optional.HasValue());

        QueryStructure queryStructure = optional.GetValue();
        _outputHelper.WriteLine(queryStructure.ToString());

        // Main properties
        Assert.Equal("490f1db4-ed14-4cdc-a09f-401048951b15", queryStructure.ClientUid);
        Assert.Equal("properties-search-query-structure", queryStructure.Name);
        Assert.Equal("Search query structure for properties", queryStructure.Description);

        // Assert AI Settings
        AiSettings aiSettings = queryStructure.AiSettings;
        Assert.Equal(100, aiSettings.UsagePercentage);
        Assert.Equal("ww-XyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyz", aiSettings.ApiKey);
        Assert.Equal("gpt-3.5-turbo-instruct", aiSettings.Model);
        Assert.Equal(0.2, aiSettings.Temperature);
        Assert.Equal(1024, aiSettings.MaxTokens);
        Assert.Equal("You are a query string builder for property search...", aiSettings.PromptTemplate);
        Assert.Equal(new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local), queryStructure.CreatedAt);
        Assert.Equal(new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local), queryStructure.UpdatedAt);

        Assert.Equal(13, queryStructure.Items.Count);

        // Asserting the first item
        Item firstItem = queryStructure.Items[0];
        Assert.Equal(0, firstItem.Rank);
        Assert.Equal("types", firstItem.Name);
        Assert.Equal("Property Type", firstItem.Description);
        Assert.Equal(12, firstItem.Entries.Count);

        // Asserting the first item's first entry
        Entry firstItemFirstEntry = firstItem.Entries[0];
        Assert.Equal(0, firstItemFirstEntry.Rank);
        Assert.Equal("ALL", firstItemFirstEntry.Key);
        Assert.True(firstItemFirstEntry.Exclusive);
        Assert.False(firstItemFirstEntry.Immiscible);
        Assert.Equal(new List<string> {"todos", "todas", "tudo", "todes", "tudinho", "tudinha"}, firstItemFirstEntry.KeyWords);
        Assert.Equal(DictionaryExtensions.Empty<string, string>(), firstItemFirstEntry.IncompatibleWith);
    }
}