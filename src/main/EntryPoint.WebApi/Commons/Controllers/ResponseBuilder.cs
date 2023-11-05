using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace EntryPoint.WebApi.Commons.Controllers;

public static class ResponseBuilder
{
    public static Task<IActionResult> BuildResponse(HttpStatusCode httpStatusCode, object payload = null)
    {
        ObjectResult objectResult = new ObjectResult(payload)
        {
            Formatters = new FormatterCollection<IOutputFormatter>(),
            DeclaredType = payload?.GetType(),
            StatusCode = (int) httpStatusCode
        };

        return Task.FromResult<IActionResult>(objectResult);
    }
}