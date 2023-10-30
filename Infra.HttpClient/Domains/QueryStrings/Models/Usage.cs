using Refit;

namespace Infra.HttpClient.Domains.QueryStrings.Models;

public sealed class Usage
{
    [AliasAs("prompt_tokens")]
    public int PromptTokens { get; set; }

    [AliasAs("completion_tokens")]
    public int CompletionTokens { get; set; }

    [AliasAs("total_tokens")]
    public int TotalTokens { get; set; }
}
