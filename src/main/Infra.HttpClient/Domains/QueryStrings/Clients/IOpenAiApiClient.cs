using System.Threading.Tasks;
using Infra.HttpClient.Domains.QueryStrings.Models;
using Refit;

namespace Infra.HttpClient.Domains.QueryStrings.Clients;

public interface IOpenAiApiClient
{
    [Post("/v1/completions")]
    [Headers("Content-Type: application/json; charset=UTF-8")]
    Task<CompletionResponse> GetCompletionAsync([Header("Authorization")] string apiKey, [Body] CompletionRequest request);
}