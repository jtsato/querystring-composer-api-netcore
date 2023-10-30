using System;
using Core.Domains.QueryStructures.Commands;
using FluentValidation;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStructures.Commands;

public class AiSettingsCommandValidatorTest
{
    private readonly ITestOutputHelper _outputHelper;
    
    public AiSettingsCommandValidatorTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate AI settings with null UsagePercentage")]
    public void FailToValidateAiSettingsWithNullUsagePercentage()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new AiSettingsCommand(
            usagePercentage: null,
            apiKey: "api_key",
            model: "model",
            temperature: "temperature",
            maxTokens: "100",
            promptTemplate: "prompt_template"
        ));
        
        _outputHelper.WriteLine(exception?.Message ?? "No exception");

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains("ValidationUsagePercentageIsNullOrEmpty", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate AI settings with invalid UsagePercentage")]
    public void FailToValidateAiSettingsWithInvalidUsagePercentage()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new AiSettingsCommand(
            usagePercentage: "invalid_percentage",
            apiKey: "api_key",
            model: "model",
            temperature: "temperature",
            maxTokens: "100",
            promptTemplate: "prompt_template"
        ));
        
        _outputHelper.WriteLine(exception?.Message ?? "No exception");

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains("ValidationUsagePercentageIsNotPercentage", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate AI settings with null ApiKey")]
    public void FailToValidateAiSettingsWithNullApiKey()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new AiSettingsCommand(
            usagePercentage: "50",
            apiKey: null,
            model: "model",
            temperature: "temperature",
            maxTokens: "100",
            promptTemplate: "prompt_template"
        ));
        
        _outputHelper.WriteLine(exception?.Message ?? "No exception");

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains("ValidationApiKeyIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationTemperatureIsNotFloat", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to validate AI settings with valid parameters")]
    public void SuccessToValidateAiSettingsWithValidParameters()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new AiSettingsCommand(
            usagePercentage: "50",
            apiKey: "api_key",
            model: "model",
            temperature: "1.0",
            maxTokens: "100",
            promptTemplate: "prompt_template"
        ));
        
        _outputHelper.WriteLine(exception?.Message ?? "No exception");

        // Assert
        Assert.Null(exception);
    }
}