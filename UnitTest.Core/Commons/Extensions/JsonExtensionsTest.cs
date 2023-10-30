using System;
using System.Text.Json;
using Core.Commons.Extensions;
using Xunit;

namespace UnitTest.Core.Commons.Extensions;

public sealed class JsonExtensionsTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to write json as string")]
    public void SuccessfulToWriteJsonAsString()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"id\": 1,  \"Name\": \"John\" }");

        // Act
        string result = jsonDocument.ToJsonString();

        // Assert
        Assert.Equal("{\"id\":1,\"Name\":\"John\"}", result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to write json as indented string")]
    public void SuccessfulToWriteJsonAsIndentedString()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"id\": 1,  \"Name\": \"John\" }");

        // Act
        string result = jsonDocument.ToJsonString(true);

        // Assert
        Assert.Equal($"{{{Environment.NewLine}  \"id\": 1,{Environment.NewLine}  \"Name\": \"John\"{Environment.NewLine}}}", result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to deserialize Json Element")]
    public void SuccessfulToDeserializeJsonElement()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"Foo\": \"Black\",  \"Bar\": \"White\" }");
        JsonElement jsonElement = jsonDocument.RootElement;

        // Act
        DummyClass result = jsonElement.ToObject<DummyClass>();

        // Assert
        Assert.Equal(new DummyClass("Black", "White"), result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to select specific token")]
    public void SuccessfulToSelectSpecificToken()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"Foo\": \"Black\",  \"Bar\": \"White\" }");
        JsonElement jsonElement = jsonDocument.RootElement;

        // Act
        JsonElement result = jsonElement.SelectToken("$.Foo");

        // Assert
        Assert.Equal("Black", result.GetString());
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to select an out of range item in array [Under Zero]")]
    public void FailToSelectAnOutOfRangeItemInArrayUnderZero()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"Foo\": \"Black\",  \"Bars\": [ { \"Name\": \"Red Lion\"} ] }");
        JsonElement jsonElement = jsonDocument.RootElement;

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            jsonElement.SelectToken("$.Bars[-1]"));

        // Assert
        Assert.Contains("The index -1 is invalid.", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to select an out of range item in array [Over Max]")]
    public void FailToSelectAnOutOfRangeItemInArrayOverMax()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"Foo\": \"Black\",  \"Bars\": [ { \"Name\": \"Red Lion\"} ] }");
        JsonElement jsonElement = jsonDocument.RootElement;

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            jsonElement.SelectToken("$.Bars[1]"));

        // Assert
        Assert.Contains("The index 1 is out of range of 'Bars' array. The max index is 0.", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to select an token item")]
    public void SuccessfulToSelectAnTokenItem()
    {
        // Arrange
        JsonDocument jsonDocument = JsonDocument.Parse("{ \"Foo\": \"Black\",  \"Bars\": [ { \"Name\": \"Red Lion\"} ] }");
        JsonElement jsonElement = jsonDocument.RootElement;

        // Act
        JsonElement result = jsonElement.SelectToken("$.Bars[0]");

        // Assert
        Assert.Equal("Red Lion", result.GetProperty("Name").GetString());
    }
}