using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.QueryStrings.Models;

public sealed class BuildQueryStringRequestBody
{
    [SwaggerSchema(Nullable = false, Description = "Search terms")]
    public string SearchTerms { get; init; }
}
