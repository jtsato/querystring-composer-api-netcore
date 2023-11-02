namespace EntryPoint.WebApi.Commons.Models;

public sealed class WebRequest : IWebRequest
{
    public string ClientUid { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}