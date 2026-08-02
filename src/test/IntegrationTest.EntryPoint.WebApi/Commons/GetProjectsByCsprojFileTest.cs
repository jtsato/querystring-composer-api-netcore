using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class GetProjectsByCsprojFileTest(ITestOutputHelper outputHelper)
{

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
            outputHelper.WriteLine($"Project name: {project.Key}");
            outputHelper.WriteLine($"Project folder: {project.Value}");
        }
    }
}