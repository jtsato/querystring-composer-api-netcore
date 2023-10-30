using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.QueryStrings.Models;

public sealed class Output
{
    public string ClientUid { get; init; }
    public string QueryName { get; init; }
    public string SearchTerms { get; init; }
    public string QueryString { get; init; }
    public bool CreatedByAi { get; init; }
    public DateTime CreatedAt { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(Output other)
    {
        return ClientUid == other.ClientUid
               && QueryName == other.QueryName
               && SearchTerms == other.SearchTerms
               && QueryString == other.QueryString
               && CreatedByAi == other.CreatedByAi
               && CreatedAt.Equals(other.CreatedAt);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Output other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(ClientUid);
        hashCode.Add(QueryName);
        hashCode.Add(SearchTerms);
        hashCode.Add(QueryString);
        hashCode.Add(CreatedByAi);
        hashCode.Add(CreatedAt);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(ClientUid)}: {ClientUid} ")
            .Append($"{nameof(QueryName)}: {QueryName} ")
            .Append($"{nameof(SearchTerms)}: {SearchTerms} ")
            .Append($"{nameof(QueryString)}: {QueryString} ")
            .Append($"{nameof(CreatedByAi)}: {CreatedByAi} ")
            .Append($"{nameof(CreatedAt)}: {CreatedAt} ")
            .ToString();
    }
}