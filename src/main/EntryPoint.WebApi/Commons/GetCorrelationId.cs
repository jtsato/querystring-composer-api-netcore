using System;
using Core.Commons;
using Microsoft.AspNetCore.Http;

namespace EntryPoint.WebApi.Commons;

public sealed class GetCorrelationId(IHttpContextAccessor httpContextAccessor) : IGetCorrelationId
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Execute()
    {
        if (_httpContextAccessor.HttpContext == null) return Guid.NewGuid().ToString();

        Optional<string> optional = CorrelationIdHeaderReader.TryGetFromHeader(_httpContextAccessor.HttpContext);
        return optional.OrElse(Guid.NewGuid().ToString());
    }
}