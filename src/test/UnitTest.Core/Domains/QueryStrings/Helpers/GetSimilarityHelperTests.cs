using Core.Domains.QueryStrings.Helpers;
using Xunit;

namespace UnitTest.Core.Domains.QueryStrings.Helpers;

public class GetSimilarityHelperTests
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "SuccessToGetSimilarity")]
    [InlineData("", "", 1)]
    [InlineData("apple", "apple", 1)]
    [InlineData("abc", "abcd", 0.75)]
    [InlineData("abcd", "abc", 0.75)]
    [InlineData("abcde", "fghij", 0)]
    [InlineData("fghij", "abcde", 0)]
    [InlineData("abc", "ab", 0.6666666666666667)]
    [InlineData("ab", "abc", 0.6666666666666667)]
    [InlineData("", "apple", 0)]
    [InlineData("apple", "", 0)]
    public void SuccessToGetSimilarity(string word, string term, double expectedSimilarity)
    {
        // Arrange
        // Act
        double actual = GetSimilarityHelper.GetSimilarity(word, term);

        // Assert
        Assert.Equal(expectedSimilarity, actual);
    }
}
