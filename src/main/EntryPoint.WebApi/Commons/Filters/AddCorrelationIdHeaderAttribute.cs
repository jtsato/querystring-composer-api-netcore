using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons.Filters;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class AddCorrelationIdHeaderAttribute : ResultFilterAttribute
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add(CorrelationIdHeader, GetCorrelationId(context));
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