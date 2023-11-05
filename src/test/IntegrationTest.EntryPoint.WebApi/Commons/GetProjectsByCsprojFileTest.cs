using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class GetProjectsByCsprojFileTest
{
    private readonly ITestOutputHelper _outputHelper;

    public GetProjectsByCsprojFileTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to get projects by csproj file")]
    public void SuccessfulToGetProjectsByCsprojFile()
    {
        // Arrange
        // Act
        IDictionary<string, string> projects = GetProjectsByCsprojFile.Projects;

        // Assert
        Assert.NotNull(projects);
        Assert.NotEmpty(projects);
        
        foreach (KeyValuePair<string, string> project in projects)
        {
            _outputHelper.WriteLine($"Project name: {project.Key}");
            _outputHelper.WriteLine($"Project folder: {project.Value}");
        }
    }
}