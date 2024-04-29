using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.QueryStrings.Models;

public sealed class OutputResponse
{
    [SwaggerSchema(Nullable = false, Description = "Client UID")]
    public string ClientUid { get; init; }
    
    [SwaggerSchema(Nullable = false, Description = "Query name")]
    public string QueryName { get; init; }
    
    [SwaggerSchema(Nullable = false, Description = "Search terms")]
    public string SearchTerms { get; init; }
    
    [SwaggerSchema(Nullable = false, Description = "Query string")]
    public string QueryString { get; init; }
    
    [SwaggerSchema(Nullable = false, Description = "Created by AI")]
    public string CreatedByAi { get; init; }
    
    [SwaggerSchema(Nullable = false, Description = "Created at")]
    public string CreatedAt { get; init; }
}
