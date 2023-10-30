using Core.Commons;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class RandomNumberGeneratorInPercentageRangeTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to generate random number in percentage range")]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(0, 3)]
    [InlineData(0, 4)]
    [InlineData(0, 5)]
    [InlineData(0, 6)]
    [InlineData(0, 7)]
    [InlineData(0, 8)]
    [InlineData(0, 9)]
    [InlineData(0, 100)]
    [InlineData(1, 1)]
    [InlineData(1, 100)]
    public void SuccessfulToGenerateRandomNumberInPercentageRange(int min, int max)
    {
        // Arrange
        // Act
        int randomNumber = RandomNumberGeneratorInPercentageRange.Generate(min, max);

        // Assert
        Assert.InRange(randomNumber, min, max);
    }
}
