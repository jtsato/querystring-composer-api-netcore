using Microsoft.Extensions.Logging;

namespace EntryPoint.WebApi.Commons;

public interface ILoggerAdapter
{
    bool IsEnabled(LogLevel logLevel);

    void LogError(string message, params object[] args);

    void LogWarning(string message, params object[] args);

    void LogInformation(string message, params object[] args);
}