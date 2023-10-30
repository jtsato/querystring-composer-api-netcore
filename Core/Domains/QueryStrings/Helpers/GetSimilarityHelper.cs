using System;
using F23.StringSimilarity;

namespace Core.Domains.QueryStrings.Helpers;

public static class GetSimilarityHelper
{
    private static readonly WeightedLevenshtein Levenshtein = new WeightedLevenshtein(new CharacterSubstitution());

    public static double GetSimilarity(string word, string term)
    {
        // Get the number of changes to transform word into term
        double distance = Levenshtein.Distance(word, term);

        // No changes needed. It means the words are equal
        if (distance == 0) return 1;

        // Get the maximum length between the two strings
        double maxLength = Math.Max(word.Length, term.Length);

        // Calculate similarity
        return 1.0 - distance / maxLength;
    }
}
