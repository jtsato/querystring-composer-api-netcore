using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons.Filters;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class GetUserActionFilter : IActionFilter
{
    private readonly IWebRequest _webRequest;

    public GetUserActionFilter(IWebRequest webRequest)
    {
        _webRequest = webRequest;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        HttpRequest request = context.HttpContext.Request;
        IHeaderDictionary headers = request.Headers;

        string token = ExtractTokenFromHeaders(headers);

        if (string.IsNullOrEmpty(token)) throw new AccessDeniedException("CommonAccessDeniedException");

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        _webRequest.ClientUid = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "azp")?.Value;
        _webRequest.Username = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
        _webRequest.Email = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private static string ExtractTokenFromHeaders(IHeaderDictionary headers)
    {   
        if (!headers.TryGetValue("authorization", out StringValues authorization)) return null;
        string[] parts = authorization.ToString().Split(' ');
        
        return parts is {Length: 2} && parts[0] == "Bearer" ? parts[1] : null;
    }
}