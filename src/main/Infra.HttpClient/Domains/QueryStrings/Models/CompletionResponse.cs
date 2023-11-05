using Refit;

namespace Infra.HttpClient.Domains.QueryStrings.Models;

public sealed class CompletionResponse
{
    [AliasAs("id")]
    public string Id { get; set; }

    [AliasAs("object")]
    public string Object { get; set; }

    [AliasAs("created")]
    public long Created { get; set; }

    [AliasAs("model")]
    public string Model { get; set; }

    [AliasAs("choices")]
    public Choice[] Choices { get; set; }

    [AliasAs("usage")]
    public Usage Usage { get; set; }
}
