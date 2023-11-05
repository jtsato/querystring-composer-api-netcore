using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Domains.QueryStrings.Helpers;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStrings.UseCases;

public static partial class ManualQueryBuilderHelper
{
    private const string Anonymous = "Anonymous";

    private static readonly string[] Separators = {" ", ", ", ". ", "", "?", ";", "", "-", "(", ")", "[", "]", "{", "}", "\"", "\""};

    [GeneratedRegex("\\s+")]
    private static partial Regex BlankSpaces();

    public static async Task<string> Build(QueryStructure queryStructure, string rawSearchTerms)
    {
        string searchTerms = NormalizeSearchTerms(rawSearchTerms);

        IList<string> words = searchTerms.ToLower().Split(Separators, StringSplitOptions.RemoveEmptyEntries).ToList();
        IList<string> allNouns = queryStructure.Items.SelectMany(element => element.Entries).SelectMany(entry => entry.KeyWords).ToList();

        IDictionary<string, string> queryParameters = new Dictionary<string, string>();
        foreach (Item item in queryStructure.Items)
        {
            string queryParameter = await BuildParameterForItem(item, allNouns, queryParameters, words);
            if (string.IsNullOrEmpty(queryParameter)) continue;
            string key = queryParameter.Split('=')[0];
            queryParameters[key] = queryParameter;
        }

        return queryParameters.Count > 0 ? "?" + string.Join("&", queryParameters.Values) : string.Empty;
    }

    private static string NormalizeSearchTerms(string rawSearchTerms)
    {
        TextElementEnumerator textEnumerator = StringInfo.GetTextElementEnumerator(rawSearchTerms);

        StringBuilder stringBuilder = new StringBuilder();
        while (textEnumerator.MoveNext())
        {
            if (char.IsSurrogatePair(textEnumerator.GetTextElement(), 0))
            {
                stringBuilder.Append($" {textEnumerator.GetTextElement()} ");
                continue;
            }

            stringBuilder.Append(textEnumerator.GetTextElement());
        }

        string result = stringBuilder.ToString();

        return BlankSpaces().Replace(result, " ");
    }

    private static Task<string> BuildParameterForItem(Item item, IList<string> allNouns, IDictionary<string, string> queryParameters, IList<string> words)
    {
        IList<Entry> entries = item.Entries;

        if (entries.Count == 0) return Task.FromResult(string.Empty);

        HashSet<string> values = item.IsCountable
            ? BuildCountableItem(item, allNouns, queryParameters, words)
            : BuildNonCountableItem(item, RephraseNumerals(item, allNouns, words));

        string result = values.Count > 0 ? $"{item.Name}={string.Join(",", values)}" : string.Empty;

        return Task.FromResult(result);
    }

    private static IList<string> RephraseNumerals(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        StringBuilder stringBuilder = new StringBuilder();

        foreach (WordInfo wordInfo in wordInfos)
        {
            stringBuilder.Append($"{wordInfo.Value.Split(' ')[0]} ");
        }

        IList<string> recycleWords = stringBuilder.ToString().Trim().Split(Separators, StringSplitOptions.RemoveEmptyEntries).ToList();

        return recycleWords;
    }

    private static HashSet<string> BuildCountableItem(Item item, IList<string> allNouns, IDictionary<string, string> queryParameters, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        IList<string> itemKeyWords = item.Entries.SelectMany(element => element.KeyWords).ToList();
        IList<WordInfo> infoList = GroupWordInfos(wordInfos, itemKeyWords, item.WaitForConfirmationWords);

        return infoList.Count == 0 ? new HashSet<string>() : FilterAndGetWordInfoValue(infoList, item, queryParameters);
    }

    private static List<WordInfo> GroupWordInfos(IList<WordInfo> wordInfos, IList<string> keyWords, bool waitForConfirmation)
    {
        List<WordInfo> wordInfoList = new List<WordInfo>();
        List<WordInfo> candidates = new List<WordInfo>();

        ProcessWordInfos(wordInfos, keyWords, waitForConfirmation, wordInfoList, candidates);

        if (!waitForConfirmation && candidates.Count > 0)
        {
            wordInfoList.AddRange(candidates);
        }

        return wordInfoList;
    }

    private static void ProcessWordInfos(IList<WordInfo> wordInfos, ICollection<string> keyWords, bool waitForConfirmation, IList<WordInfo> wordInfoList, IList<WordInfo> candidates)
    {
        int lastPosition = wordInfos.Count - 1;

        for (int index = lastPosition; index >= 0; index--)
        {
            WordInfo wordInfo = wordInfos[index];

            if (HandleQuantifiedNoun(wordInfo, keyWords, candidates)) continue;
            if (HandleNoun(wordInfo, keyWords, wordInfoList, waitForConfirmation)) continue;
            if (HandleConfirmationIndicator(wordInfo, candidates, wordInfoList)) continue;

            HandleRevocationIndicator(wordInfo, candidates);
        }
    }

    private static bool HandleQuantifiedNoun(WordInfo wordInfo, ICollection<string> allKeyWords, ICollection<WordInfo> candidates)
    {
        if (wordInfo.Type != WordInfoType.QuantifiedNoun) return false;

        string noun = wordInfo.Value.Split(' ')[1];
        if ((allKeyWords.Count != 0 || noun != Anonymous) && !allKeyWords.Contains(noun)) return false;

        candidates.Add(wordInfo);

        return true;
    }

    private static bool HandleNoun(WordInfo wordInfo, ICollection<string> allKeyWords, IList<WordInfo> wordInfoList, bool waitForConfirmation)
    {
        if (wordInfo.Type != WordInfoType.Noun) return false;

        if (waitForConfirmation || !allKeyWords.Contains(wordInfo.Value)) return true;

        bool shouldIncrement = wordInfoList.Count > 0 && wordInfoList[^1].Type == WordInfoType.QuantifiedNoun
                                                      && wordInfoList[^1].Value.Split(' ')[1] == wordInfo.Value;

        if (shouldIncrement)
        {
            WordInfo previousWordInfo = wordInfoList[^1];
            string numberAsString = previousWordInfo.Value.Split(' ')[0];
            long number = Convert.ToInt64(numberAsString);
            previousWordInfo.Value = $"{number + 1} {wordInfo.Value}";
            return true;
        }

        wordInfo.Type = WordInfoType.QuantifiedNoun;
        wordInfo.Value = $"1 {wordInfo.Value}";

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

    private static HashSet<string> FilterAndGetWordInfoValue(IEnumerable<WordInfo> wordInfoList, Item item, IDictionary<string, string> queryParameters)
    {
        List<string> values = wordInfoList.Select(info => info.Value).ToList();

        // A garage for two cars is two garages.
        values.Reverse();

        foreach (string value in values)
        {
            string numberAsString = value.Split(' ')[0];
            string noun = value.Split(' ')[1];
            IEnumerable<Entry> entries = item.Entries.Where(entry => entry.KeyWords.Contains(noun));

            foreach (Entry entry in entries)
            {
                long number = NumberConverterHelper.ConvertWordsToNumber(numberAsString);
                if (IsIncompatible(entry, queryParameters)) continue;

                return number != 0 ? new HashSet<string>(new[] {Convert.ToString(number)}) : new HashSet<string>();
            }
        }

        return new HashSet<string>();
    }

    private static bool IsIncompatible(Entry entry, IDictionary<string, string> queryParameters1)
    {
        bool isIncompatible = false;

        foreach (KeyValuePair<string, string> parameter in queryParameters1)
        {
            if (!entry.IncompatibleWith.TryGetValue(parameter.Key, out string incompatibleKeyValue)) continue;
            string parameterValue = parameter.Value.Split('=')[1];
            if (!string.Equals(parameterValue, incompatibleKeyValue, StringComparison.InvariantCultureIgnoreCase)) continue;
            isIncompatible = true;
        }

        return isIncompatible;
    }

    private static HashSet<string> BuildNonCountableItem(Item item, IList<string> words)
    {
        return PickupMultipleEntryKeys(item.Entries, words);
    }

    private static HashSet<string> PickupMultipleEntryKeys(IList<Entry> entries, IList<string> words)
    {
        Dictionary<Entry, int> entrySimilarity = new Dictionary<Entry, int>();

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

        Dictionary<Entry, int> topEntrySimilarity = entrySimilarity.Where(keyValuePair => keyValuePair.Value >= 75)
            .OrderByDescending(keyValuePair => keyValuePair.Value)
            .ToDictionary(keyValuePair => keyValuePair.Key, pair => pair.Value);

        if (!topEntrySimilarity.Any()) return new HashSet<string>();

        // Get all entries with the Immiscible attribute
        List<KeyValuePair<Entry, int>> immiscibleEntries = topEntrySimilarity.Where(keyValuePair
            => keyValuePair.Key.Immiscible).ToList();

        // If all top similar entries are Immiscible, return the one with the highest similarity
        if (immiscibleEntries.Count == topEntrySimilarity.Count)
        {
            Entry immiscibleEntry = immiscibleEntries[0].Key;
            return new HashSet<string>(new[] {immiscibleEntry.Key});
        }

        // If there are any Immiscible entries and there's more than one entry with high similarity, remove them
        if (immiscibleEntries.Any() && topEntrySimilarity.Count > 1)
        {
            foreach (KeyValuePair<Entry, int> immiscibleEntry in immiscibleEntries)
            {
                topEntrySimilarity.Remove(immiscibleEntry.Key);
            }
        }

        // Check the remaining top entry
        Entry topEntry = topEntrySimilarity.First().Key;

        // If it's exclusive, return the key
        return topEntry.Exclusive
            ? new HashSet<string>(new[] {topEntry.Key})
            // Otherwise, return all keys with the similarity greater than 75%
            : new HashSet<string>(topEntrySimilarity.Keys.Select(entry => entry.Key));
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