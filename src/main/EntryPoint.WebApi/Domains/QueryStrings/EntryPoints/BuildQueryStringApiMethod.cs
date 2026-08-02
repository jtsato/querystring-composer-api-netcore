using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Core.Commons;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.QueryStrings.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.QueryStrings.EntryPoints;

[ApiController]
[Route("v1/compositions")]
[ApiExplorerSettings(GroupName = "Compositions")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class BuildQueryStringApiMethod(IBuildQueryStringController controller) : IApiMethod
{
    private readonly IBuildQueryStringController _controller = ArgumentValidator.CheckNull(controller, nameof(controller));

    [SwaggerOperation(
        Summary = "Build query string",
        Description = "Build query string",
        OperationId = "BuildQueryString",
        Tags = ["Compositions"]
    )]
    [ProducesResponseType(typeof(OutputResponse), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseStatus), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResponseStatus), (int) HttpStatusCode.InternalServerError)]
    [HttpPost("{queryName}")]
    [SuppressMessage("Usage", "ASP0018:Unused route parameter", Justification = "Bound via [FromRoute(Name = \"queryName\")] on BuildQueryStringRequest.QueryName")]
    public async Task<IActionResult> BuildQueryString(BuildQueryStringRequest request)
    {
        return await _controller.ExecuteAsync(request);
    }
}
