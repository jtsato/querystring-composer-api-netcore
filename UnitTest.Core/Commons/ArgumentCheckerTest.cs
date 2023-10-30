using System;
using System.Collections.Generic;
using Core.Commons;
using UnitTest.Core.Commons.Models;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class ArgumentCheckerTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as percentage")]
    [InlineData("100", true)]
    [InlineData("0", true)]
    [InlineData("101", false)]
    [InlineData("abc", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    public void SuccessfulToValidateAnStringParameterAsPercentage(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsPercentage(input));
    }
    
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as byte")]
    [InlineData("123", true)]
    [InlineData("abc123", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("256", false)]
    public void SuccessfulToValidateAnStringParameterAsByte(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsByte(input));
    }
    
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as integer")]
    [InlineData("123", true)]
    [InlineData("abc", false)]
    [InlineData("abc123", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("NaN", false)]
    public void SuccessfulToValidateAnStringParameterAsInteger(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsInteger(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as long")]
    [InlineData("123", true)]
    [InlineData("abc", false)]
    [InlineData("abc123", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("NaN", false)]
    public void SuccessfulToValidateAnStringParameterAsLong(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsLong(input));
    }
    
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as float")]
    [InlineData("123", true)]
    [InlineData("123.45", true)]
    [InlineData("abc", false)]
    [InlineData("abc123", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("NaN", true)]
    public void SuccessfulToValidateAnStringParameterAsFloat(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsFloat(input));
    }
    
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as double")]
    [InlineData("123", true)]
    [InlineData("123.45", true)]
    [InlineData("abc", false)]
    [InlineData("abc123", false)]
    [InlineData(",.%$#@", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("NaN", true)]
    public void SuccessfulToValidateAnStringParameterAsDouble(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsDouble(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an string parameter as a valid url")]
    [InlineData("/", false)]
    [InlineData("www.google.com.br", false)]
    [InlineData("ftp://www.google.com.br", false)]
    [InlineData("http://www.google.com.br", true)]
    [InlineData("https://www.google.com.br", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void SuccessfulToValidateAnStringParameterAsAValidUrl(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsValidUri(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate duplicity over an object enumerable")]
    [InlineData(new[] {1, 2, 2, 2}, true)]
    [InlineData(new[] {1, 2, 3}, false)]
    [InlineData(new int[] { }, false)]
    public void SuccessfulToValidateDuplicityOverAnObjectEnumerable(IEnumerable<int> input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.HasDuplicatedValues(input));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate duplicity over a null object")]
    public void FailToValidateDuplicityOverANullObject()
    {
        // Arrange
        // Act
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(
                () => ArgumentChecker.HasDuplicatedValues((IEnumerable<int>) null)
            );

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'source')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate a date")]
    [InlineData("2021-04-23T10:00:01.000Z", true)]
    [InlineData("2021-13-32T25:61:61.000Z", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    public void SuccessfulToValidateADate(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.BeEmptyOrAValidDate(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate an enumeration")]
    [InlineData("BLUE", true)]
    [InlineData("PURPLE", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void SuccessfulToValidateAnEnumeration(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsValidEnumOf<Color>(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate a json")]
    [InlineData("{}", true)]
    [InlineData("{\"name\":\"John\",\"age\":30,\"car\":null,\"friends\":[{\"name\":\"Peter\",\"age\":29,\"car\":null}]}", true)]
    [InlineData(null, false)]
    [InlineData("Json", false)]
    [InlineData("", false)]
    public void SuccessfulToValidateAJson(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.IsJson(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate if json schema have all required schema fields")]
    [InlineData("{}", false)]
    [InlineData(@"{""properties"": {""name"": {""type"": ""string""}}, ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""string""}}}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""string""}}, ""required"": []}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""string""}}, ""required"": [""name""]}", true)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""employee"": {""type"": ""string""}}, ""required"": [""name""]}", true)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""invalidType""}}, ""required"": [""name""]}", false)]
    public void SuccessfulToValidateIfJsonSchemaHaveAllRequiredSchemaFields(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.HaveAllRequiredSchemaFields(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate if json schema have all properties typed")]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": """"}, ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""notType"": ""string""}}, ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": ""string""}, ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""notInteger""}}, ""required"": [""name""]}", false)]
    [InlineData(@"{""type"": ""object"", ""properties"": {""name"": {""type"": ""string""}}, ""required"": [""name""]}", true)]
    [InlineData(@"{""type"": ""object"", ""properties"": { ""employee"": { ""type"": ""object"", ""properties"": { ""id"": { ""notAType"": ""integer"" } }, ""required"": [""id""] }}, ""required"": [""employee""]}", false)]
    public void SuccessfulToValidateIfJsonSchemaHaveAllPropertiesTyped(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.HaveAllPropertiesTyped(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate if json schema have all required schema fields")]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("0", false)]
    public void FailToValidateIfJsonSchemaHaveAllRequiredSchemaFields(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.HaveAllRequiredSchemaFields(input));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate if json schema have all properties typed")]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("0", false)]
    public void FailToValidateIfJsonSchemaHaveAllPropertiesTyped(string input, bool expected)
    {
        Assert.Equal(expected, ArgumentChecker.HaveAllPropertiesTyped(input));
    }
}