using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.QueryStrings.Models;

public sealed class OutputResponse
{
    [SwaggerSchema(Description = "Client UID")]
    public string ClientUid { get; init; }
    
    [SwaggerSchema(Description = "Query name")]
    public string QueryName { get; init; }
    
    [SwaggerSchema(Description = "Search terms")]
    public string SearchTerms { get; init; }
    
    [SwaggerSchema(Description = "Query string")]
    public string QueryString { get; init; }
    
    [SwaggerSchema(Description = "Created by AI")]
    public bool CreatedByAi { get; init; }
    
    [SwaggerSchema(Description = "Created at")]
    public string CreatedAt { get; init; }
}
