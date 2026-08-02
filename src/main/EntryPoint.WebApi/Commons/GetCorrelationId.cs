using System;
using System.Linq;
using Core.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons;

public sealed class GetCorrelationId(IHttpContextAccessor httpContextAccessor) : IGetCorrelationId
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public string Execute()
    {
        if (httpContextAccessor.HttpContext == null) return Guid.NewGuid().ToString();

        Optional<string> optional = TryGetCorrelationIdFromHeader(httpContextAccessor.HttpContext);
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