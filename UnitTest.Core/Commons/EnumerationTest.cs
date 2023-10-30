using System.Linq;
using Core.Commons;
using UnitTest.Core.Commons.Models;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class EnumerationTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get all enumeration items")]
    public void SuccessfulToGetAllEnumerationItems()
    {
        // Arrange
        // Act
        Color[] colors = Enumeration<Color>.GetAll().ToArray();

        // Assert
        Assert.Equal(5, colors.Length);
        Assert.Contains(Color.Blue, colors);
        Assert.Contains(Color.Green, colors);
        Assert.Contains(Color.Yellow, colors);
        Assert.Contains(Color.Red, colors);
        Assert.Contains(Color.Black, colors);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get enumeration item by name")]
    public void SuccessfulToGetEnumerationItemByName()
    {
        // Arrange
        // Act
        Optional<Color> optional = Enumeration<Color>.GetByName("BLACK");

        // Assert
        Assert.True(optional.HasValue());

        Color black = optional.GetValue();

        Assert.Equal("BLACK", black.Name);
        Assert.False(Color.Black.Equals(null));
        Assert.Equal(Color.Black, black);
        Assert.True(Color.Black.Equals(black));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to compare enumeration items")]
    [InlineData(true, "BLACK", "BLACK")]
    [InlineData(false, "BLACK", "RED")]
    public void SuccessfulToCompareEnumerationItems(bool expected, string colorName1, string colorName2)
    {
        // Arrange
        // Act
        Optional<Color> optional1 = Enumeration<Color>.GetByName(colorName1);
        Optional<Color> optional2 = Enumeration<Color>.GetByName(colorName2);

        Assert.True(optional1.HasValue());
        Assert.True(optional2.HasValue());

        Color color1 = optional1.GetValue();
        Color color2 = optional2.GetValue();

        // Assert
        Assert.Equal(expected, color1.Equals(color2));
        Assert.Equal(expected, color1.GetHashCode().Equals(color2.GetHashCode()));
        Assert.Equal(expected, color1.ToString()!.Equals(color2.ToString()));

        Assert.Equal(expected, color1.Is(color2));
        Assert.Equal(!expected, color1.IsNot(color2));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to get enumeration item by name")]
    public void FailToGetEnumerationItemByName()
    {
        // Arrange
        // Act
        Optional<Color> optional = Enumeration<Color>.GetByName("PURPLE");

        // Assert
        Assert.False(optional.HasValue());
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to get enumeration item by name with null value")]
    public void FailToGetEnumerationItemByNameWithNullValue()
    {
        // Arrange
        // Act
        Optional<Color> optional = Enumeration<Color>.GetByName(null);

        // Assert
        Assert.False(optional.HasValue());
    }
}