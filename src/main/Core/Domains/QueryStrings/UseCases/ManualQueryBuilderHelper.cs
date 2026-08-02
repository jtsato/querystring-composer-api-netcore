using System;
using System.Collections.Generic;
using System.Linq;
using Core.Commons;
using Core.Domains.QueryStrings.Helpers;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStrings.UseCases;

// Builds a query string from free text, deterministically and without any AI involved.
//
// The search terms are normalized once into words, then every item of the query structure is resolved
// against those words in rank order: non-countable items first, because a countable item can be ruled
// out by a non-countable one that was already resolved.
public static class ManualQueryBuilderHelper
{
    public static string Build(QueryStructure queryStructure, string rawSearchTerms)
    {
        using IDisposable similarityCache = GetSimilarityHelper.BeginCache();

        IList<string> words = SearchTermsNormalizer.ToWords(rawSearchTerms);

        IList<string> allNouns =
        [
            .. queryStructure.Items
                .SelectMany(item => item.Entries)
                .SelectMany(entry => entry.KeyWords)
        ];

        SentenceParsingContext parsingContext = SentenceParserHelper.CreateContext(words, allNouns);

        Dictionary<string, QueryParameter> queryParameters = new Dictionary<string, QueryParameter>();

        foreach (Item item in ItemsByRank(queryStructure, countable: false))
        {
            Add(queryParameters, item, NonCountableItemResolver.Resolve(item, parsingContext));
        }

        foreach (Item item in ItemsByRank(queryStructure, countable: true))
        {
            Add(queryParameters, item, CountableItemResolver.Resolve(item, parsingContext, queryParameters));
        }

        return queryParameters.Count > 0
            ? "?" + string.Join("&", queryParameters.Values)
            : string.Empty;
    }

    private static IEnumerable<Item> ItemsByRank(QueryStructure queryStructure, bool countable)
    {
        return queryStructure.Items
            .Where(item => item.IsCountable == countable)
            .OrderBy(item => item.Rank);
    }

    private static void Add(Dictionary<string, QueryParameter> queryParameters, Item item, Optional<QueryParameter> resolved)
    {
        if (resolved.HasValue()) queryParameters[item.Name] = resolved.GetValue();
    }
}
