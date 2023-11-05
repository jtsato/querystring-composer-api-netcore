using System;
using System.Collections.Generic;
using Core.Commons;
using UnitTest.Core.Commons.Models;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class ArgumentValidatorTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to validate an object parameter if it is not null")]
    public void SuccessfulToValidateAnObjectParameterIfItIsNotNull()
    {
        // Arrange
        DummyClass dummyObject = new DummyClass("X", "Y");

        // Act
        DummyClass result = ArgumentValidator.CheckNull(dummyObject, nameof(dummyObject));

        // Assert
        Assert.Equal(new DummyClass("X", "Y"), result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object parameter if it is null")]
    public void FailToValidateAnObjectParameterIfItIsNull()
    {
        // Arrange
        // Act
        // Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(
                () => ArgumentValidator.CheckNull((object) null, "dummyObject")
            );
        Assert.Equal("Value cannot be null. (Parameter 'dummyObject')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object parameter if it is null with custom message")]
    public void FailToValidateAnObjectParameterIfItIsNullWithCustomMessage()
    {
        // Arrange
        // Act
        // Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(
                () => ArgumentValidator.CheckNull((object) null, "dummy", "Dummy cannot be null")
            );
        Assert.Equal("Dummy cannot be null", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to validate an object list parameter if it is not null")]
    public void SuccessfulToValidateAnObjectListParameterIfItIsNotNull()
    {
        // Arrange
        List<DummyClass> dummyObjects = new List<DummyClass>
            {new DummyClass("X", "Y")};

        // Act
        IReadOnlyCollection<DummyClass> result = ArgumentValidator.CheckNullOrEmpty(dummyObjects, nameof(dummyObjects));

        // Assert
        Assert.Equal(new List<DummyClass> {new DummyClass("X", "Y")}, result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object list parameter if it is null")]
    public void FailToValidateAnObjectListParameterIfItIsNull()
    {
        // Arrange
        // Act
        // Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(
                () => ArgumentValidator.CheckNullOrEmpty((IReadOnlyCollection<DummyClass>) null, "dummyCollection")
            );
        Assert.Equal("Value cannot be null. (Parameter 'dummyCollection')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object list parameter if it is empty")]
    public void FailToValidateAnObjectListParameterIfItIsEmpty()
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNullOrEmpty(new List<DummyClass>(), "dummyCollection")
            );
        Assert.Equal("Value cannot be null or empty. (Parameter 'dummyCollection')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object list parameter if it is null with custom message")]
    public void FailToValidateAnObjectListParameterIfItIsNullWithCustomMessage()
    {
        // Arrange
        // Act
        // Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(
                () => ArgumentValidator.CheckNullOrEmpty((IReadOnlyCollection<DummyClass>) null, "dummyCollection", "Dummy collection cannot be null")
            );
        Assert.Equal("Dummy collection cannot be null", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate an object list parameter if it is empty with custom message")]
    public void FailToValidateAnObjectListParameterIfItIsEmptyWithCustomMessage()
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNullOrEmpty(new List<DummyClass>(), "dummyCollection", "Dummy collection cannot be empty")
            );
        Assert.Equal("Dummy collection cannot be empty", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to validate a string parameter if it is not null")]
    public void SuccessfulToValidateAStringParameterIfItIsNotNull()
    {
        // Arrange
        // Act
        string result = ArgumentValidator.CheckEmpty("dummyString", "dummyString");

        // Assert
        Assert.Equal("dummyString", result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate a string parameter if it is null")]
    public void FailToValidateAStringParameterIfItIsNull()
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckEmpty(null, "dummyString")
            );
        Assert.Equal("Value cannot be null or empty. (Parameter 'dummyString')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to validate a string parameter if it is null with custom message")]
    public void FailToValidateAStringParameterIfItIsNullWithCustomMessage()
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckEmpty(null, "dummy", "Dummy cannot be empty")
            );
        Assert.Equal("Dummy cannot be empty", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate negative parameters")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void SuccessfulToValidateNegativeParameters(int input)
    {
        ArgumentValidator.CheckNegative(input, "Input", $"The {input} is not a valid positive integer");
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate negative parameters")]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    public void FailToValidateNegativeParameters(int input)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNegative(input, nameof(input), string.Empty)
            );

        Assert.Equal($"Value cannot be negative. (Parameter '{nameof(input)}')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate negative parameters with custom message")]
    [InlineData(int.MinValue, "Input cannot be negative")]
    [InlineData(-1, "Input cannot be negative")]
    public void FailToValidateNegativeParametersWithCustomMessage(int input, string message)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNegative(input, nameof(input), message)
            );

        Assert.Equal(message, exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate negative or equals to zero parameter with custom message")]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void FailToValidateNegativeOrEqualsToZeroParameterWithCustomMessage(int input)
    {
        ArgumentValidator.CheckNegativeOrZero(input, "Input", $"The {input} is not a valid positive integer");
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate negative or equals to zero parameter")]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    public void FailToValidateNegativeOrEqualsToZeroParameter(int input)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNegativeOrZero(input, nameof(input), string.Empty)
            );

        Assert.Equal($"Value cannot be negative or equals to zero. (Parameter '{nameof(input)}')", exception.Message);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to validate NegativeOrZero parameters with custom message")]
    [InlineData(int.MinValue, "Input cannot be negative or equals to zero")]
    [InlineData(-1, "Input cannot be negative or equals to zero")]
    public void FailToValidateNegativeOrZeroParametersWithCustomMessage(int input, string message)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => ArgumentValidator.CheckNegativeOrZero(input, nameof(input), message)
            );

        Assert.Equal(message, exception.Message);
    }
}