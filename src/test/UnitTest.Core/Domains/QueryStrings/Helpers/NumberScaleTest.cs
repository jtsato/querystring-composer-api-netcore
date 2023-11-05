using Core.Domains.QueryStrings.Helpers;
using Xunit;

namespace UnitTest.Core.Domains.QueryStrings.Helpers;

public class NumberScaleTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Success to get scale by number")]
    [InlineData(1, "Units")]
    [InlineData(9, "Units")]
    [InlineData(10, "Tens")]
    [InlineData(99, "Tens")]
    [InlineData(100, "Hundreds")]
    [InlineData(999, "Hundreds")]
    [InlineData(1000, "Thousands")]
    [InlineData(999999, "Thousands")]
    [InlineData(1000000, "Millions")]
    [InlineData(999999999, "Millions")]
    [InlineData(1000000000, "Billions")]
    public void SuccessToGetScaleByNumber(long number, string expectedScaleName)
    {
        // Arrange
        // Act
        NumberScale scale = NumberScale.GetScaleByNumber(number);

        // Assert
        Assert.Equal(expectedScaleName, scale.Name);
    }
}