using System;
using Core.Domains.Clients.Commands;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.Clients.Commands;

public sealed class RegisterClientCommandTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to register client with null parameters")]
    public void FailToRegisterClientWithNullParameters()
    {
        // Arrange
        // Act
        Exception exception = Record.Exception(() => new RegisterClientCommand(null, null, null));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
        Assert.Contains("ValidationClientUidIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationNameIsNullOrEmpty", exception.Message);
        Assert.Contains("ValidationDescriptionIsNullOrEmpty", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to register client command")]
    public void SuccessToRegisterClientCommand()
    {
        // Arrange
        // Act
        RegisterClientCommand command = new RegisterClientCommand(
            uid: "9419357e-123b-494a-8bc3-fd17373c218b",
            name: "White Duck Home",
            description: "White Duck Home is a platform that brings together properties in one place."
        );

        // Assert
        Assert.NotNull(command);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", command.Uid);
        Assert.Equal("White Duck Home", command.Name);
        Assert.Equal("White Duck Home is a platform that brings together properties in one place.", command.Description);
    }
}