using Microsoft.AspNetCore.Http;

namespace EntryPoint.WebApi.Commons.Controllers;

public static class UrlHelper
{
    private const string HttpScheme = "http";
    private const int DefaultHttpPort = 80;
    private const string HttpsScheme = "https";
    private const int DefaultHttpsPort = 443;

    public static string GetBaseUrl(HttpContext httpContext)
    {
        HttpRequest httpRequest = httpContext.Request;
        string scheme = httpRequest.Scheme;
        string host = httpRequest.Host.Host;
        string port = GetPort(httpRequest.Scheme, httpRequest.Host.Port);
        string pathBase = httpRequest.PathBase;
        string path = httpRequest.Path;

        return $"{scheme}://{host}{port}{pathBase}{path}";
    }

    private static string GetPort(string scheme, int? port)
    {
        if (port is null || (HttpScheme.Equals(scheme) && DefaultHttpPort.Equals(port)) || (HttpsScheme.Equals(scheme) && DefaultHttpsPort.Equals(port)))
        {
            return string.Empty;
        }

        return $":{port}";
    }
}