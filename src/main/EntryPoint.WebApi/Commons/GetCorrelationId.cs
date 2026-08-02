using System;
using Core.Commons;
using Microsoft.AspNetCore.Http;

namespace EntryPoint.WebApi.Commons;

public sealed class GetCorrelationId(IHttpContextAccessor httpContextAccessor) : IGetCorrelationId
{
    public string Execute()
    {
        if (httpContextAccessor.HttpContext == null) return Guid.NewGuid().ToString();

        Optional<string> optional = CorrelationIdHeaderReader.TryGetFromHeader(httpContextAccessor.HttpContext);
        return optional.OrElse(Guid.NewGuid().ToString());
    }
}