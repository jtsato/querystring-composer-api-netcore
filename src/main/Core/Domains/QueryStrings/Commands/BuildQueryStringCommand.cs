using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.QueryStrings.Commands;

public sealed class BuildQueryStringCommand
{
    private static readonly BuildQueryStringCommandValidator Validator = new BuildQueryStringCommandValidator();

    public string ClientUid { get; init; }
    public string QueryName { get; }
    public string SearchTerms { get; }

    public BuildQueryStringCommand(string clientUid, string queryName, string searchTerms)
    {
        ClientUid = clientUid;
        QueryName = queryName;
        SearchTerms = searchTerms;
        Validator.ValidateAndThrow(this);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(BuildQueryStringCommand other)
    {
        return ClientUid == other.ClientUid
               && QueryName == other.QueryName
               && SearchTerms == other.SearchTerms;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is BuildQueryStringCommand other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(ClientUid);
        hashCode.Add(QueryName);
        hashCode.Add(SearchTerms);

        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(ClientUid)}: {ClientUid} ")
            .Append($"{nameof(QueryName)}: {QueryName} ")
            .Append($"{nameof(SearchTerms)}: {SearchTerms} ")
            .ToString();
    }
}