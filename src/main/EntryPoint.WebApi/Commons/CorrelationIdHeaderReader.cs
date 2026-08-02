using System.Linq;
using Core.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons;

internal static class CorrelationIdHeaderReader
{
    public const string HeaderName = "X-Correlation-Id";

    public static Optional<string> TryGetFromHeader(HttpContext context)
    {
        IHeaderDictionary headers = context.Request.Headers;
        if (!headers.TryGetValue(HeaderName, out StringValues values)) return Optional<string>.Empty();

        string correlationId = values.ToList()[0];
        return !string.IsNullOrWhiteSpace(correlationId) ? Optional<string>.Of(correlationId) : Optional<string>.Empty();
    }
}
