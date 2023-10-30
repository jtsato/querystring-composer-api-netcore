using System.Threading.Tasks;
using Infra.HttpClient.Domains.QueryStrings.Models;
using Refit;

namespace Infra.HttpClient.Domains.QueryStrings.Clients;

public interface IOpenAiApiClient
{
    [Post("/v1/completions")]
    Task<CompletionResponse> GetCompletionAsync([Body] CompletionRequest request);
}