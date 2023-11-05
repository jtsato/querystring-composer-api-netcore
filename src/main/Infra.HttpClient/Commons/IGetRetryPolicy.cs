namespace Infra.HttpClient.Commons;

public interface IGetRetryPolicy
{
    int Attempts { get; }

    int IntervalSeconds { get; }
}