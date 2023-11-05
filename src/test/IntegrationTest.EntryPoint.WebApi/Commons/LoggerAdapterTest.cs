using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EntryPoint.WebApi.Commons;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
public sealed class LoggerAdapterTest
{
    private readonly Mock<ILogger> _logger;

    private readonly ILoggerAdapter _loggerAdapter;

    public LoggerAdapterTest()
    {
        _logger = new Mock<ILogger>(MockBehavior.Strict);

        Mock<ILoggerFactory> loggerFactory = new Mock<ILoggerFactory>(MockBehavior.Strict);

        loggerFactory
            .Setup(factory => factory.CreateLogger(typeof(LoggerAdapterTest).FullName))
            .Returns(_logger.Object);

        _loggerAdapter = new LoggerAdapter<LoggerAdapterTest>(loggerFactory.Object);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to log an error message")]
    public void SuccessfulToLogAnErrorMessage()
    {
        // Arrange
        _logger
            .Setup(logger1 => logger1.IsEnabled(LogLevel.Error))
            .Returns(true);

        _logger
            .Setup(logger1 => logger1.Log
                (
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                )
            )
            .Verifiable();

        // Act
        _loggerAdapter.LogError("An unexpected error has occurred, please try again later");

        // Assert
        Assert.True(_loggerAdapter.IsEnabled(LogLevel.Error));

        _logger.Verify(loggerFactory => loggerFactory.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((valueObject, _) => LogMessageMatcher(valueObject, "An unexpected error has occurred, please try again later")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once
        );
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to log a warning message")]
    public void SuccessfulToLogAWarningMessage()
    {
        // Arrange
        _logger
            .Setup(logger1 => logger1.IsEnabled(LogLevel.Warning))
            .Returns(true);

        _logger
            .Setup(logger1 => logger1.Log
                (
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
            )
            .Verifiable();

        // Act
        _loggerAdapter.LogWarning("It doesn't pay to ignore warnings. Even when they don’t make sense");

        // Assert
        Assert.True(_loggerAdapter.IsEnabled(LogLevel.Warning));

        _logger.Verify(loggerFactory => loggerFactory.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((valueObject, _) => LogMessageMatcher(valueObject, "It doesn't pay to ignore warnings. Even when they don’t make sense")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once
        );
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to log an information message")]
    public void SuccessfulToLogAnInformationMessage()
    {
        // Arrange
        _logger
            .Setup(logger1 => logger1.IsEnabled(LogLevel.Information))
            .Returns(true);

        _logger
            .Setup(logger1 => logger1.Log
                (
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
            )
            .Verifiable();

        // Act
        _loggerAdapter.LogInformation("Information is not knowledge");

        // Assert
        Assert.True(_loggerAdapter.IsEnabled(LogLevel.Information));

        _logger.Verify(loggerFactory => loggerFactory.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((valueObject, _) => LogMessageMatcher(valueObject, "Information is not knowledge")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once
        );
    }

    private static bool LogMessageMatcher(object valueObject, string expected)
    {
        IReadOnlyList<KeyValuePair<string, object>> loggedValueObject = valueObject as IReadOnlyList<KeyValuePair<string, object>>;
        string actual = loggedValueObject!.FirstOrDefault(keyValuePair => keyValuePair.Key == "{OriginalFormat}").Value.ToString();
        return actual == expected;
    }
}