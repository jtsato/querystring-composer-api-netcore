using System;
using Core.Domains.QueryStrings.Commands;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.QueryStrings.Commands;

public sealed class BuildQueryStringCommandTest
{
    [Trait("Category", "Core Business Tests")]
    [Theory(DisplayName = "Fail to build query string when parameters are invalid")]
    [InlineData(null, "queryName", "searchTerms", "ValidationClientUidIsNullOrEmpty")]
    [InlineData("", "queryName", "searchTerms", "ValidationClientUidIsNullOrEmpty")]
    [InlineData("clientUid", null, "searchTerms", "ValidationQueryNameIsNullOrEmpty")]
    [InlineData("clientUid", "", "searchTerms", "ValidationQueryNameIsNullOrEmpty")]
    [InlineData("clientUid", "queryName", null, "ValidationSearchTermsIsNullOrEmpty")]
    [InlineData("clientUid", "queryName", "", "ValidationSearchTermsIsNullOrEmpty")]
    public void FailToBuildQueryStringWhenParametersAreInvalid(string clientUid, string queryName, string searchTerms, string expected)
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new BuildQueryStringCommand(clientUid, queryName, searchTerms));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains(expected, exception.Message);
    }
    
    [Trait("Category", "Core Business Tests")]
    [Theory(DisplayName = "Build query string when parameters are valid")]
    [InlineData("clientUid", "queryName", "searchTerms")]
    public void BuildQueryStringWhenParametersAreValid(string clientUid, string queryName, string searchTerms)
    {
        // Arrange
        // Act
        BuildQueryStringCommand command = new BuildQueryStringCommand(clientUid, queryName, searchTerms);

        // Assert
        Assert.NotNull(command);
        Assert.Equal(clientUid, command.ClientUid);
        Assert.Equal(queryName, command.QueryName);
        Assert.Equal(searchTerms, command.SearchTerms);
    }
}