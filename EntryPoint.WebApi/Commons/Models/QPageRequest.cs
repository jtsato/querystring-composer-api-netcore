using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Commons.Models;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class QPageRequest
{
    [SwaggerParameter(Required = false, Description = "Results page you want to retrieve (0..N)")]
    [FromQuery(Name = "page")]
    public string PageNumber { get; init; }

    [SwaggerParameter(Required = false, Description = "Number of records per page")]
    [FromQuery(Name = "pageSize")]
    public string PageSize { get; init; }

    [SwaggerParameter(Required = false,
        Description = "Sorting criteria in the format: property(:asc|desc). " +
                      "Default sort order is ascending. " +
                      "Multiple sort criteria are supported. ")]
    [FromQuery(Name = "orderBy")]
    public List<string> OrderBy { get; init; }
}