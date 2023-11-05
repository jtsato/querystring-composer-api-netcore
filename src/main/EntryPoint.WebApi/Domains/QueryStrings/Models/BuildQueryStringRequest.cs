using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.QueryStrings.Models;

public sealed class BuildQueryStringRequest
{
    [SwaggerParameter(Required = true, Description = "Query name identifier")]
    [FromRoute(Name = "queryName")]
    public string QueryName { get; init; }
    
    [SwaggerParameter(Required = true, Description = "Search terms")]
    [FromBody]
    public BuildQueryStringRequestBody Body { get; init; }
}
