using System;
using Core.Domains.QueryStructures.Commands;
using FluentValidation;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStructures.Commands;

public sealed class RegisterQueryStructureCommandValidatorTest
{
    private readonly ITestOutputHelper _outputHelper;
    
    public RegisterQueryStructureCommandValidatorTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to register query structure with null parameters")]
    public void FailToRegisterQueryStructureWithNullParameters()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new RegisterQueryStructureCommand(null, null, null, null));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains("ValidationClientUidIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationNameIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationDescriptionIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationAiSettingsIsNull", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to register query structure command")]
    public void SuccessToRegisterQueryStructureCommand()
    {
        // Arrange
        // Act
        RegisterQueryStructureCommand command = new RegisterQueryStructureCommand
        (
            clientUid: "9419357e-123b-494a-8bc3-fd17373c218b",
            name: "White Duck Home",
            description: "White Duck Home is a platform that brings together properties in one place.",
            aiSettings: new AiSettingsCommand(
                usagePercentage: "50",
                apiKey: "api_key",
                model: "model",
                temperature: "1.0",
                maxTokens: "100",
                promptTemplate: "prompt_template"
            )
        );
        
        _outputHelper.WriteLine(command.ToString());

        // Assert
        Assert.NotNull(command);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", command.ClientUid);
        Assert.Equal("White Duck Home", command.Name);
        Assert.Equal("White Duck Home is a platform that brings together properties in one place.", command.Description);
        
        Assert.NotNull(command.AiSettings);
        Assert.Equal("50", command.AiSettings.UsagePercentage);
        Assert.Equal("api_key", command.AiSettings.ApiKey);
        Assert.Equal("model", command.AiSettings.Model);
        Assert.Equal("1.0", command.AiSettings.Temperature);
        Assert.Equal("100", command.AiSettings.MaxTokens);
        Assert.Equal("prompt_template", command.AiSettings.PromptTemplate);
    }
}