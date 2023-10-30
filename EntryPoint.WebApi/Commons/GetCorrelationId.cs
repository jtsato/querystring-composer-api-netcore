using System;
using System.Linq;
using Core.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons;

public sealed class GetCorrelationId : IGetCorrelationId
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCorrelationId(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Execute()
    {
        if (_httpContextAccessor.HttpContext == null) return Guid.NewGuid().ToString();

        Optional<string> optional = TryGetCorrelationIdFromHeader(_httpContextAccessor.HttpContext);
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