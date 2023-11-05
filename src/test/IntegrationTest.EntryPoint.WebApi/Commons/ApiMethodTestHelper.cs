using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

public static class ApiMethodTestHelper
{
    public static JsonElement TryGetJsonElement(ObjectResult objectResult)
    {
        JsonSerializerOptions options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        string jsonAsString = JsonSerializer.Serialize(objectResult.Value, options);

        JsonDocument jsonDocument = JsonDocument.Parse(jsonAsString);
        Assert.NotNull(jsonDocument);

        return jsonDocument.RootElement;
    }
}