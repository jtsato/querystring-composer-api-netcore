using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons.Filters;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    private readonly List<Type> _businessExceptions = new List<Type>
    {
        typeof(AccessDeniedException),
        typeof(InvalidArgumentException),
        typeof(NotFoundException),
        typeof(ParentConstraintException),
        typeof(UniqueConstraintException),
        typeof(ValidationException)
    };

    private readonly IExceptionHandler _exceptionHandler;
    private readonly ILoggerAdapter _logger;

    public ExceptionHandlerFilterAttribute(IExceptionHandler exceptionHandler, ILoggerAdapter logger)
    {
        _exceptionHandler = ArgumentValidator.CheckNull(exceptionHandler, nameof(exceptionHandler));
        _logger = ArgumentValidator.CheckNull(logger, nameof(logger));
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        string correlationId = GetCorrelationId(context);
        context.HttpContext.Response.Headers.Add(CorrelationIdHeader, correlationId);
        LogException(correlationId, context.Exception);
        context.Result = await _exceptionHandler.HandleAsync(context.Exception);
        context.ExceptionHandled = true;
    }

    public override void OnException(ExceptionContext context)
    {
        string correlationId = GetCorrelationId(context);
        context.HttpContext.Response.Headers.Add(CorrelationIdHeader, correlationId);
        LogException(correlationId, context.Exception);
        context.Result = _exceptionHandler.HandleAsync(context.Exception).Result;
        context.ExceptionHandled = true;
    }

    private void LogException(string correlationId, Exception exception)
    {
        if (_businessExceptions.Contains(exception.GetType()))
        {
            if (!_logger.IsEnabled(LogLevel.Warning)) return;
            _logger.LogWarning("{CorrelationIdHeader}: {CorrelationId}, {Exception}: {Message}", CorrelationIdHeader, correlationId, exception.GetType(), exception.Message);
            return;
        }

        if (!_logger.IsEnabled(LogLevel.Error)) return;
        _logger.LogError("{CorrelationIdHeader}: {CorrelationId}, {Exception}: {Message}", CorrelationIdHeader, correlationId, exception.GetType(), exception.Message);
    }

    private static string GetCorrelationId(ActionContext context)
    {
        Optional<string> optional = TryGetCorrelationIdFromHeader(context.HttpContext);
        return optional.OrElse(Guid.NewGuid().ToString());
    }

    private static Optional<string> TryGetCorrelationIdFromHeader(HttpContext context)
    {
        IHeaderDictionary headers = context.Request.Headers;
        if (!headers.TryGetValue(CorrelationIdHeader, out StringValues values)) return Optional<string>.Empty();

        string correlationId = values.ToList()[0];
        return !string.IsNullOrWhiteSpace(correlationId) ? Optional<string>.Of(correlationId) : Optional<string>.Empty();
    }
}