using System.Collections.Generic;
using Core.Commons;
using Core.Commons.Models;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class JsonSchemaValidatorTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate json against the schema")]
    [InlineData
    (
        @"{ ""employeeName"": ""Arthur"", ""department"": ""Tech"" }",
        @"{""type"": ""object"", ""properties"": {""employeeName"": {""type"": ""string""},""department"": {""type"": ""string""}},""required"": [""employeeName"",""department""]}"
    )]
    [InlineData
    (
        @"{ ""projectName"": ""Falcon 6"", ""priority"": 1 }",
        @"{""type"": ""object"", ""properties"": {""projectName"": {""type"": ""string""},""priority"": {""type"": ""integer""}},""required"": [""projectName"",""priority""]}"
    )]
    public void SuccessfulToValidateJsonAgainstTheSchema(string json, string jsonSchema)
    {
        Assert.Empty(JsonSchemaValidator.Validate(json, jsonSchema));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to list field errors if fail to validate json against the schema")]
    public void SuccessfulToListFieldErrorsIfFailToValidateJsonAgainstTheSchema()
    {
        // Arrange
        // Act
        IList<FieldError> fieldErrors = JsonSchemaValidator.Validate(@"{ ""employeeName"": ""Arthur"", ""age"": 29 }",
            @"{""type"": ""object"", ""properties"": {""employeeName"": {""type"": ""string""},""department"": {""type"": ""string""}},""required"": [""employeeName"",""department""]}");

        // Assert
        Assert.NotEmpty(fieldErrors);
        Assert.Equal(2, fieldErrors.Count);

        Assert.Equal("department", fieldErrors[0].PropertyName);
        Assert.Null(fieldErrors[0].AttemptedValue);
        Assert.Equal("CommonJsonPropertyMissing", fieldErrors[0].ErrorMessage);

        Assert.Equal("age", fieldErrors[1].PropertyName);
        Assert.Equal("29", fieldErrors[1].AttemptedValue);
        Assert.Equal("CommonJsonPropertyInvalid", fieldErrors[1].ErrorMessage);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to list field errors if fail to match field type")]
    public void SuccessfulToListFieldErrorsIfFailToMatchFieldType()
    {
        // Arrange
        // Act
        IList<FieldError> fieldErrors = JsonSchemaValidator.Validate(@"{ ""employeeName"": ""Arthur"", ""age"": ""Not an integer"", ""department"": true }",
            @"{""type"": ""object"", ""properties"": {""employeeName"": {""type"": ""string""},""age"": {""type"": ""integer""},""department"": {""type"": ""string""}},""required"": [""employeeName"",""age"", ""department""]}");

        // Assert
        Assert.NotEmpty(fieldErrors);
        Assert.Equal(2, fieldErrors.Count);

        Assert.Equal("age", fieldErrors[0].PropertyName);
        Assert.Equal("Not an integer", fieldErrors[0].AttemptedValue);
        Assert.Equal("CommonJsonPropertyWrongType", fieldErrors[0].ErrorMessage);

        Assert.Equal("department", fieldErrors[1].PropertyName);
        Assert.Equal("An invalid value", fieldErrors[1].AttemptedValue);
        Assert.Equal("CommonJsonPropertyWrongType", fieldErrors[1].ErrorMessage);
    }
}