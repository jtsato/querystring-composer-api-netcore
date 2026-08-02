using System;
using System.Collections.Generic;
using System.Linq;
using Core.Commons;
using Core.Domains.QueryStrings.Helpers;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStrings.UseCases;

// Resolves an item that carries a quantity, such as the minimum number of bedrooms.
//
// The sentence is walked from right to left so that a quantifier can be confirmed or revoked by the
// words that follow it: "no mínimo 2 quartos" confirms, "até 2 quartos" revokes.
public static class CountableItemResolver
{
    private const string Anonymous = "Anonymous";

    public static Optional<QueryParameter> Resolve(Item item, IList<string> allNouns, IList<string> words,
        IReadOnlyDictionary<string, QueryParameter> resolvedParameters)
    {
        if (item.Entries.Count == 0) return Optional<QueryParameter>.Empty();

        List<WordInfo> quantifiedNouns = CollectQuantifiedNouns(item, allNouns, words);
        if (quantifiedNouns.Count == 0) return Optional<QueryParameter>.Empty();

        HashSet<string> values = PickupFirstCompatibleCount(quantifiedNouns, item, resolvedParameters);

        return values.Count > 0
            ? Optional<QueryParameter>.Of(new QueryParameter(item.Name, values))
            : Optional<QueryParameter>.Empty();
    }

    private static List<WordInfo> CollectQuantifiedNouns(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        IList<string> itemKeyWords = [.. item.Entries.SelectMany(entry => entry.KeyWords)];

        return GroupWordInfos(wordInfos, itemKeyWords, item.WaitForConfirmationWords);
    }

    private static List<WordInfo> GroupWordInfos(IList<WordInfo> wordInfos, ICollection<string> keyWords, bool waitForConfirmation)
    {
        List<WordInfo> wordInfoList = [];
        List<WordInfo> candidates = [];

        ProcessWordInfos(wordInfos, keyWords, waitForConfirmation, wordInfoList, candidates);

        if (!waitForConfirmation && candidates.Count > 0)
        {
            wordInfoList.AddRange(candidates);
        }

        return wordInfoList;
    }

    private static void ProcessWordInfos(IList<WordInfo> wordInfos, ICollection<string> keyWords, bool waitForConfirmation,
        IList<WordInfo> wordInfoList, ICollection<WordInfo> candidates)
    {
        int lastPosition = wordInfos.Count - 1;

        for (int index = lastPosition; index >= 0; index--)
        {
            WordInfo wordInfo = wordInfos[index];

            if (wordInfo.Type == WordInfoType.QuantifiedNoun)
            {
                if (!HandleQuantifiedNoun(wordInfo, keyWords, candidates)) CloseClauseBoundary(waitForConfirmation, wordInfoList, candidates);
                continue;
            }

            if (wordInfo.Type == WordInfoType.Noun && index > 0 && wordInfos[index - 1].Type == WordInfoType.QuantifiedNoun) continue;
            if (HandleNoun(wordInfo, keyWords, wordInfoList, waitForConfirmation)) continue;
            if (HandleConfirmationIndicator(wordInfo, candidates, wordInfoList)) continue;

            HandleRevocationIndicator(wordInfo, candidates);
        }
    }

    private static bool HandleQuantifiedNoun(WordInfo wordInfo, ICollection<string> allKeyWords, ICollection<WordInfo> candidates)
    {
        string noun = QuantifiedNoun.Parse(wordInfo.Value).Noun;
        if ((allKeyWords.Count != 0 || noun != Anonymous) && !allKeyWords.Contains(noun)) return false;

        candidates.Add(wordInfo);

        return true;
    }

    // A quantified noun that belongs to a different item ("5000 reais" while resolving area) marks the
    // edge of this item's clause: whatever was gathered so far can no longer be confirmed or revoked by
    // indicator words on the other side of it, so it is settled here instead.
    private static void CloseClauseBoundary(bool waitForConfirmation, IList<WordInfo> wordInfoList, ICollection<WordInfo> candidates)
    {
        if (!waitForConfirmation)
        {
            foreach (WordInfo candidate in candidates) wordInfoList.Add(candidate);
        }

        candidates.Clear();
    }

    private static bool HandleNoun(WordInfo wordInfo, ICollection<string> allKeyWords, IList<WordInfo> wordInfoList, bool waitForConfirmation)
    {
        if (wordInfo.Type != WordInfoType.Noun) return false;

        if (waitForConfirmation || !allKeyWords.Contains(wordInfo.Value)) return true;

        // A bare noun counts as one of itself, and repeating it adds up: "quarto quarto" means two bedrooms.
        bool shouldIncrement = wordInfoList.Count > 0
                               && wordInfoList[^1].Type == WordInfoType.QuantifiedNoun
                               && QuantifiedNoun.Parse(wordInfoList[^1].Value).Noun == wordInfo.Value;

        if (shouldIncrement)
        {
            WordInfo previousWordInfo = wordInfoList[^1];
            previousWordInfo.Value = QuantifiedNoun.Parse(previousWordInfo.Value).Increment().ToString();

            return true;
        }

        wordInfo.Type = WordInfoType.QuantifiedNoun;
        wordInfo.Value = QuantifiedNoun.Of(1, wordInfo.Value).ToString();
        wordInfoList.Add(wordInfo);

        return true;
    }

    private static bool HandleConfirmationIndicator(WordInfo wordInfo, ICollection<WordInfo> candidates, ICollection<WordInfo> wordInfoList)
    {
        if (wordInfo.Type != WordInfoType.ConfirmationIndicator) return false;

        foreach (WordInfo candidate in candidates) wordInfoList.Add(candidate);
        candidates.Clear();

        return true;
    }

    private static void HandleRevocationIndicator(WordInfo wordInfo, ICollection<WordInfo> candidates)
    {
        if (wordInfo.Type == WordInfoType.RevocationIndicator)
        {
            candidates.Clear();
        }
    }

    private static HashSet<string> PickupFirstCompatibleCount(IEnumerable<WordInfo> wordInfoList, Item item,
        IReadOnlyDictionary<string, QueryParameter> queryParameters)
    {
        List<QuantifiedNoun> quantifiedNouns =
        [
            .. wordInfoList
                .Select(wordInfo => QuantifiedNoun.Parse(wordInfo.Value))
        ];

        // A garage for two cars is two garages.
        quantifiedNouns.Reverse();

        foreach (QuantifiedNoun quantifiedNoun in quantifiedNouns)
        {
            IEnumerable<Entry> entries = item.Entries.Where(entry => entry.KeyWords.Contains(quantifiedNoun.Noun));

            foreach (Entry entry in entries)
            {
                if (IsIncompatible(entry, queryParameters)) continue;

                return quantifiedNoun.Count != 0
                    ? [quantifiedNoun.CountAsText]
                    : [];
            }
        }

        return [];
    }

    // "Garagem no centro" asks for a property in the Centro district that has a garage, while
    // "com garagem" asks for at least one garage. The entry declares which already resolved parameter
    // makes it redundant.
    private static bool IsIncompatible(Entry entry, IReadOnlyDictionary<string, QueryParameter> queryParameters)
    {
        bool isIncompatible = false;

        foreach (KeyValuePair<string, QueryParameter> parameter in queryParameters)
        {
            if (!entry.IncompatibleWith.TryGetValue(parameter.Key, out string incompatibleValue)) continue;
            if (!string.Equals(parameter.Value.ValuesAsText, incompatibleValue, StringComparison.InvariantCultureIgnoreCase)) continue;
            isIncompatible = true;
        }

        return isIncompatible;
    }
}
