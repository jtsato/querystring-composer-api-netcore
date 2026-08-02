namespace Infra.HttpClient.Commons;

public class GetRetryPolicy(int attempts, int intervalSeconds) : IGetRetryPolicy
{
    public int Attempts { get; } = attempts;

    public int IntervalSeconds { get; } = intervalSeconds;

}
