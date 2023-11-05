using Core.Commons;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class GetDateTimeTest
{
    private readonly IGetDateTime _getDateTime = new GetDateTime();

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get current date and time")]
    private void SuccessfulToGetCurrentDateAndTime()
    {
        Assert.NotEqual(_getDateTime.Now(), _getDateTime.Now());
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get current utc date and time")]
    private void SuccessfulToGetCurrentUtcDateAndTime()
    {
        Assert.NotEqual(_getDateTime.UtcNow(), _getDateTime.UtcNow());
    }
}