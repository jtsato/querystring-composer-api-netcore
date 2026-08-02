using System;
using System.Collections.Generic;
using System.Linq;
using Core.Commons;
using Core.Domains.QueryStrings.Helpers;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStrings.UseCases;

// Resolves an item whose entries are picked by similarity rather than counted, such as the property
// type or the district.
//
// Every entry is scored against every word; entries above the similarity floor are kept and then
// filtered by the Exclusive and Immiscible flags.
public static class NonCountableItemResolver
{
    private const int SimilarityFloorInPercentage = 80;

    public static Optional<QueryParameter> Resolve(Item item, IList<string> allNouns, IList<string> words)
    {
        if (item.Entries.Count == 0) return Optional<QueryParameter>.Empty();

        HashSet<string> keys = PickupMultipleEntryKeys(item.Entries, ToLeadingTokens(item, allNouns, words));

        return keys.Count > 0
            ? Optional<QueryParameter>.Of(new QueryParameter(item.Name, keys))
            : Optional<QueryParameter>.Empty();
    }

    // The sentence parser merges a quantifier with the noun it qualifies into a single "3 quartos"
    // value. A non-countable item matches single words only, so each parsed value is reduced back to
    // its leading token.
    private static List<string> ToLeadingTokens(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        return
        [
            .. wordInfos
                .Select(wordInfo => wordInfo.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
        ];
    }

    private static HashSet<string> PickupMultipleEntryKeys(ICollection<Entry> entries, IList<string> words)
    {
        Dictionary<Entry, int> entrySimilarity = new Dictionary<Entry, int>(entries.Count);

        foreach (Entry entry in entries)
        {
            double maxSimilarity = 0;
            foreach (string word in words)
            {
                double similarity = GetMaxSimilarity(entry.KeyWords, word);
                if (similarity <= maxSimilarity) continue;
                maxSimilarity = similarity;
            }

            entrySimilarity.Add(entry, (int) (maxSimilarity * 100));
        }

        IDictionary<Entry, int> topEntrySimilarity = entrySimilarity
            .Where(keyValuePair => keyValuePair.Value >= SimilarityFloorInPercentage)
            .OrderByDescending(keyValuePair => keyValuePair.Value)
            .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

        if (!topEntrySimilarity.Any()) return [];

        List<KeyValuePair<Entry, int>> immiscibleEntries =
        [
            .. topEntrySimilarity
                .Where(keyValuePair => keyValuePair.Key.Immiscible)
        ];

        // When every top entry is immiscible there is nothing to keep them apart from, so the best one wins.
        if (immiscibleEntries.Count == topEntrySimilarity.Count)
        {
            return [immiscibleEntries[0].Key.Key];
        }

        // Otherwise an immiscible entry drops out as soon as it has company.
        if (immiscibleEntries.Any() && topEntrySimilarity.Count > 1)
        {
            foreach (KeyValuePair<Entry, int> immiscibleEntry in immiscibleEntries)
            {
                topEntrySimilarity.Remove(immiscibleEntry.Key);
            }
        }

        Entry topEntry = topEntrySimilarity.First().Key;

        // An exclusive entry covers the others, so it answers alone.
        return topEntry.Exclusive
            ? [topEntry.Key]
            : [.. topEntrySimilarity.Keys.Select(entry => entry.Key)];
    }

    private static double GetMaxSimilarity(IEnumerable<string> words, string term)
    {
        double maxSimilarity = 0;

        foreach (string word in words)
        {
            double similarity = GetSimilarityHelper.GetSimilarity(word, term);
            if (similarity > maxSimilarity) maxSimilarity = similarity;
            if (maxSimilarity.CompareTo(1) == 0) break;
        }

        return maxSimilarity;
    }
}
