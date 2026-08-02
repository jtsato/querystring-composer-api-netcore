using System;
using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntryPoint.WebApi.Commons.Filters;

[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class AddCorrelationIdHeaderAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers[CorrelationIdHeaderReader.HeaderName] = GetCorrelationId(context);
    }

    private static string GetCorrelationId(ActionContext context)
    {
        Optional<string> optional = CorrelationIdHeaderReader.TryGetFromHeader(context.HttpContext);
        return optional.OrElse(Guid.NewGuid().ToString());
    }
}