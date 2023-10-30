using Infra.HttpClient.Commons;
using Xunit;

namespace Integration.Infra.HttpClient.Commons;

public sealed class GetRetryPolicyTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to create a retry policy")]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    public void SuccessfulToCreateARetryPolicy(int attempts, int intervalSeconds)
    {
        IGetRetryPolicy getRetryPolicy = new GetRetryPolicy(attempts, intervalSeconds);

        Assert.Equal(attempts, getRetryPolicy.Attempts);
        Assert.Equal(intervalSeconds, getRetryPolicy.IntervalSeconds);
    }
}
