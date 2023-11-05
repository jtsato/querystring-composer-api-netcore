using System.Diagnostics.CodeAnalysis;
using System.Net;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntryPoint.WebApi.Commons.Filters;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class HandleInvalidModelStateActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;

        const int httpStatusCode = (int) HttpStatusCode.BadRequest;
        string message = IExceptionHandler.MessageFormatter(nameof(Messages.CommonInvalidRequest), null);
        ResponseStatus responseStatus = new ResponseStatus(httpStatusCode, message);

        context.Result = new ObjectResult(responseStatus) {StatusCode = httpStatusCode};
    }
}