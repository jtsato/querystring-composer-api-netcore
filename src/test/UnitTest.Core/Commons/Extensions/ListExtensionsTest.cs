using Core.Commons.Extensions;
using Xunit;

namespace UnitTest.Core.Commons.Extensions;

public sealed class ListExtensionsTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to returns an empty list if the list is null")]
    [InlineData(null, new string[] { })]
    [InlineData(new string[] { }, new string[] { })]
    [InlineData(new[] {"a", "b", "c"}, new[] {"a", "b", "c"})]
    public void SuccessfulToReturnsAnEmptyListIfTheListIsNull(object list, string[] expected)
    {
        string[] array = list as string[];
        Assert.Equal(expected, array.AsEmptyIfNull());
    }
}