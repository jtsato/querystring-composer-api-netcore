using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Extensions;
using Core.Domains.QueryStrings.Gateways;
using Core.Domains.QueryStructures.Models;
using Infra.HttpClient.Commons;
using Infra.HttpClient.Domains.QueryStrings.Clients;
using Infra.HttpClient.Domains.QueryStrings.Models;
using Polly;
using Polly.Retry;

namespace Infra.HttpClient.Domains.QueryStrings.Providers;

public sealed class BuildQueryStringWithAiProvider : IBuildQueryStringWithAiGateway
{
    private readonly IOpenAiApiClient _openAiApiClient;
    private readonly AsyncRetryPolicy _retryPolicy;

    public BuildQueryStringWithAiProvider(IOpenAiApiClient openAiApiClient, IGetRetryPolicy getRetryPolicy)
    {
        _openAiApiClient = ArgumentValidator.CheckNull(openAiApiClient, nameof(openAiApiClient));
        _retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(getRetryPolicy.Attempts, _ => TimeSpan.FromSeconds(getRetryPolicy.IntervalSeconds));
    }

    public async Task<Optional<string>> ExecuteAsync(QueryStructure queryStructure, string searchTerm)
    {
        return await _retryPolicy.ExecuteAsync(async () => await GetCompletionAsync(queryStructure, searchTerm));
    }

    private async Task<Optional<string>> GetCompletionAsync(QueryStructure queryStructure, string searchTerm)
    {
        CompletionRequest request = new CompletionRequest
        {
            Model = queryStructure.AiSettings.Model,
            Temperature = queryStructure.AiSettings.Temperature,
            MaxTokens = queryStructure.AiSettings.MaxTokens,
            Prompt = CreatePrompt(queryStructure.AiSettings, searchTerm),
        };
        
        string apiKey = $"Bearer {queryStructure.AiSettings.ApiKey}";
        CompletionResponse completionResponse = await _openAiApiClient.GetCompletionAsync(apiKey, request);
        string response = completionResponse?.Choices?.FirstOrDefault()?.Text?.SubstringAfter("?").Trim();
        
        return string.IsNullOrWhiteSpace(response) ? Optional<string>.Empty() : Optional<string>.Of($"?{response}");
    }

    private static string CreatePrompt(AiSettings aiSettings, string searchTerm)
    {
        return aiSettings.PromptTemplate.Replace("{{searchTerm}}", searchTerm);
    }
}
