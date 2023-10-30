using System;
using Core.Commons.Extensions;
using Xunit;

namespace UnitTest.Core.Commons.Extensions;

public sealed class StringExtensionsTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to gets the substring before the first separator occurrence")]
    [InlineData(null, "*", null)]
    [InlineData("", "*", "")]
    [InlineData("abc", "a", "")]
    [InlineData("abcba", "b", "a")]
    [InlineData("abc", "c", "ab")]
    [InlineData("abc", "d", "abc")]
    [InlineData("abc", "", "")]
    [InlineData("abc", null, "abc")]
    public void SuccessfulToGetsTheSubstringBeforeTheFirstSeparatorOccurrence(string text, string stopAt, string expected)
    {
        Assert.Equal(expected, text.SubstringBefore(stopAt));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to gets the substring before the last separator occurrence")]
    [InlineData(null, "*", null)]
    [InlineData("", "*", "")]
    [InlineData("abc", "a", "")]
    [InlineData("abcba", "b", "abc")]
    [InlineData("abc", "c", "ab")]
    [InlineData("abc", "d", "abc")]
    [InlineData("abc", "", "abc")]
    [InlineData("abc", null, "abc")]
    [InlineData("abcxyz", "xyz", "abc")]
    [InlineData("abcxyz", "", "abcxyz")]
    [InlineData("abcxyz", "?", "abcxyz")]
    public void SuccessfulToGetsTheSubstringBeforeTheLastSeparatorOccurrence(string text, string stopAt, string expected)
    {
        Assert.Equal(expected, text.SubstringBeforeLast(stopAt));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to gets the substring after the first separator occurrence")]
    [InlineData(null, "*", null)]
    [InlineData("abc", null, "abc")]
    [InlineData("", "*", "")]
    [InlineData("c", "c", "")]
    [InlineData("abc", "a", "bc")]
    [InlineData("abcba", "b", "cba")]
    [InlineData("abc", "c", "")]
    [InlineData("abc", "d", "")]
    [InlineData(" abc", " ", "abc")]
    public void SuccessfulToGetsTheSubstringAfterTheFirstSeparatorOccurrence(string text, string stopAt, string expected)
    {
        Assert.Equal(expected, text.SubstringAfter(stopAt));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to gets the substring after the last separator occurrence")]
    [InlineData(null, "*", null)]
    [InlineData("abc", null, "abc")]
    [InlineData("", "*", "")]
    [InlineData("c", "c", "")]
    [InlineData("abc", "a", "bc")]
    [InlineData("abcba", "b", "a")]
    [InlineData("abc", "c", "")]
    [InlineData("abc", "bc", "")]
    [InlineData("abc", "d", "")]
    [InlineData(" abc", " ", "abc")]
    [InlineData("abcxyz", "abc", "xyz")]
    [InlineData("abcxyz", "xyz", "")]
    [InlineData("abcxyz", "", "abcxyz")]
    [InlineData("abcxyz", "?", "")]
    public void SuccessfulToGetsTheSubstringAfterTheLastSeparatorOccurrence(string text, string stopAt, string expected)
    {
        Assert.Equal(expected, text.SubstringAfterLast(stopAt));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to append suffix if missing")]
    [InlineData(null, null, false, null)]
    [InlineData(null, "*", false, null)]
    [InlineData("abc", null, false, "abc")]
    [InlineData("", "xyz", false, "xyz")]
    [InlineData("abc", "xyz", false, "abcxyz")]
    [InlineData("abcxyz", "xyz", false, "abcxyz")]
    [InlineData("abcXYZ", "xyz", false, "abcXYZxyz")]
    [InlineData("abcXYZ", "xyz", true, "abcXYZ")]
    public void SuccessfulToAppendSuffixIfMissing(string text, string suffix, bool ignoreCase, string expected)
    {
        Assert.Equal(expected, text.AppendIfMissing(suffix, ignoreCase));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to truncate string")]
    [InlineData(null, 0, null)]
    [InlineData("", 0, "")]
    [InlineData("", 2, "")]
    [InlineData("abc", 0, "")]
    [InlineData("abc", 2, "ab")]
    [InlineData("abc", 4, "abc")]
    public void SuccessfulToTruncateString(string text, int maxWidth, string expected)
    {
        Assert.Equal(expected, text.Truncate(maxWidth));
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to truncate string")]
    [InlineData("abc", -1)]
    public void FailToTruncateString(string text, int maxWidth)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(
                () => text.Truncate(maxWidth)
            );

        Assert.Equal("Max width must be greater than or equal to zero.", exception.Message);
    }
}