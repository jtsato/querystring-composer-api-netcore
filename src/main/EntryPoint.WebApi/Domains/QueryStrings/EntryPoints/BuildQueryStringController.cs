using System.Net;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStrings.Commands;
using Core.Domains.QueryStrings.Interfaces;
using Core.Domains.QueryStrings.Models;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.QueryStrings.Models;
using EntryPoint.WebApi.Domains.QueryStrings.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.QueryStrings.EntryPoints;

public sealed class BuildQueryStringController(IWebRequest webRequest, IBuildQueryStringUseCase useCase) : IBuildQueryStringController
{
    private readonly IWebRequest _webRequest = ArgumentValidator.CheckNull(webRequest, nameof(webRequest));
    private readonly IBuildQueryStringUseCase _useCase = ArgumentValidator.CheckNull(useCase, nameof(useCase));

    public async Task<IActionResult> ExecuteAsync(BuildQueryStringRequest request)
    {
        BuildQueryStringCommand command = new BuildQueryStringCommand
        (
            clientUid: _webRequest.ClientUid,
            queryName: request.QueryName,
            searchTerms: request.Body.SearchTerms
        );
        
        Output output = await _useCase.ExecuteAsync(command);
        OutputResponse response = OutputResponsePresenter.Map(output);
        
        return await ResponseBuilder.BuildResponse(HttpStatusCode.OK, response);
    }
}
