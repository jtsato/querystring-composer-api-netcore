using Core.Commons.Models;
using Xunit;

namespace UnitTest.Core.Commons.Models;

public class RangeTests
{
    public static TheoryData<Range<int>, Range<int>, bool> EqualRangesTestData =>
        new TheoryData<Range<int>, Range<int>, bool>
        {
            {Range<int>.Of(1, 5), Range<int>.Of(1, 5), true},
            {Range<int>.Of(1, 5), Range<int>.Of(3, 7), false},
        };

    public static TheoryData<Range<int>, string> ToStringTestData =>
        new TheoryData<Range<int>, string>
        {
            {Range<int>.Of(1, 5), "From: 1, To: 5"},
            {default(Range<int>), "From: 0, To: 0"},
            {Range<int>.Of(1, 1), "From: 1, To: 1"},
        };

    [Theory]
    [MemberData(nameof(EqualRangesTestData))]
    public void ShouldReturnEqualRangesIfSameFromAndToValues(Range<int> range1, Range<int> range2, bool expectedResult)
    {
        // Arrange
        // Act
        // Assert
        Assert.Equal(expectedResult, range1.Equals(range2));
    }

    [Theory]
    [MemberData(nameof(ToStringTestData))]
    public void ToString_ShouldReturnCorrectStringRepresentation(Range<int> range, string expectedString)
    {
        // Arrange
        // Act
        string actual = range.ToString();

        // Assert
        Assert.Equal(expectedString, actual);
    }

    [Theory]
    [MemberData(nameof(EqualRangesTestData))]
    public void GetHashCode_ShouldReturnSameHashForEqualRanges(Range<int> range1, Range<int> range2, bool expectedResult)
    {
        // Arrange
        // Act
        int hash1 = range1.GetHashCode();
        int hash2 = range2.GetHashCode();

        // Assert
        Assert.Equal(expectedResult, hash1 == hash2);
    }

    [Theory]
    [MemberData(nameof(EqualRangesTestData))]
    public void OperatorEquality_ShouldReturnCorrectEqualityResult(Range<int> range1, Range<int> range2, bool expectedResult)
    {
        // Arrange
        // Act
        bool actualResult = range1 == range2;

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [MemberData(nameof(EqualRangesTestData))]
    public void OperatorInequality_ShouldReturnCorrectInequalityResult(Range<int> range1, Range<int> range2, bool expectedResult)
    {
        // Arrange
        // Act
        bool actualResult = range1 != range2;

        // Assert
        Assert.Equal(!expectedResult, actualResult);
    }
}