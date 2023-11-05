namespace Infra.HttpClient.Commons;

public class GetRetryPolicy : IGetRetryPolicy
{
    public int Attempts { get; }

    public int IntervalSeconds { get; }

    public GetRetryPolicy(int attempts, int intervalSeconds)
    {
        Attempts = attempts;
        IntervalSeconds = intervalSeconds;
    }
}
