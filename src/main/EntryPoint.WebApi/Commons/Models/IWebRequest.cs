namespace EntryPoint.WebApi.Commons.Models;

public interface IWebRequest
{
    string ClientUid { get; set; }
    string Username { get; set; }
    string Email { get; set; }
}