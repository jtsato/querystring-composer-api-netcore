using Refit;

namespace Infra.HttpClient.Domains.QueryStrings.Models;

public sealed class Choice
{
    [AliasAs("text")]
    public string Text { get; set; }

    [AliasAs("index")]
    public int Index { get; set; }

    [AliasAs("logprobs")]
    public object Logprobs { get; set; }

    [AliasAs("finish_reason")]
    public string FinishReason { get; set; }
}