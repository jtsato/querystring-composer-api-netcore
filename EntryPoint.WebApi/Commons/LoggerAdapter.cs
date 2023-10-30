using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace EntryPoint.WebApi.Commons;

[SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
public class LoggerAdapter<T> : ILoggerAdapter
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.Log(LogLevel.Error, message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.Log(LogLevel.Warning, message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.Log(LogLevel.Information, message, args);
    }
}