using System;
using System.Collections.Generic;
using F23.StringSimilarity;

namespace Core.Domains.QueryStrings.Helpers;

public static class GetSimilarityHelper
{
    private static readonly WeightedLevenshtein Levenshtein = new WeightedLevenshtein(new CharacterSubstitution());
    private const float Epsilon = 0.0001f;

    // A single ManualQueryBuilderHelper.Build call parses the same words against the same nouns once
    // per item of the query structure, so the exact same (word, term) pair is re-scored many times over.
    // Scoping the cache to the thread for the lifetime of one Build call (via BeginCache/CacheScope)
    // avoids recomputing it without risking unbounded growth across unrelated requests.
    [ThreadStatic]
    private static Dictionary<(string Word, string Term), double> _cache;

    public static IDisposable BeginCache()
    {
        _cache = new Dictionary<(string, string), double>();

        return new CacheScope();
    }

    public static double GetSimilarity(string word, string term)
    {
        if (_cache is not null && _cache.TryGetValue((word, term), out double cached)) return cached;

        double similarity = ComputeSimilarity(word, term);

        _cache?.Add((word, term), similarity);

        return similarity;
    }

    private static double ComputeSimilarity(string word, string term)
    {
        // Get the number of changes to transform word into term
        double distance = Levenshtein.Distance(word, term);

        // No changes needed. It means the words are equal
        if (distance < Epsilon) return 1.0;

        // Get the maximum length between the two strings
        double maxLength = Math.Max(word.Length, term.Length);

        // Calculate similarity
        return 1.0 - distance / maxLength;
    }

    private sealed class CacheScope : IDisposable
    {
        public void Dispose()
        {
            _cache = null;
        }
    }
}
