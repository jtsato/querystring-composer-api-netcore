using System.Threading.Tasks;
using EntryPoint.WebApi.Domains.QueryStrings.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.Commons;

public interface IQueryStringController
{
}

public interface IBuildQueryStringController : IQueryStringController
{
    Task<IActionResult> ExecuteAsync(BuildQueryStringRequest request);
}