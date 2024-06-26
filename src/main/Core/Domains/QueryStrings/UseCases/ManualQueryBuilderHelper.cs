﻿using System;
using System.Collections.Concurrent;
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

    private static readonly string[] ReplaceBySpace = {". ", ",00", "?", ";", "-", "(", ")", "[", "]", "{", "}", "\"", "/", "\\"};
    private static readonly string[] ReplaceByEmpty = {"."};
    private static readonly string[] Separators = {" ", ","};

    [GeneratedRegex("\\s+")]
    private static partial Regex BlankSpaces();

    [GeneratedRegex(@"\d+(?!\d)")]
    private static partial Regex LastNumberOfNumericSequence();

    public static Task<string> Build(QueryStructure queryStructure, string rawSearchTerms)
    {
        string searchTerms = AddSpaceBeforeFirstNumber(AddSpaceAfterLastNumber(NormalizeSearchTerms(rawSearchTerms)));

        IList<string> words = searchTerms.ToLower().Split(Separators, StringSplitOptions.RemoveEmptyEntries).ToList();
        
        IList<string> allNouns = queryStructure.Items
            .SelectMany(element => element.Entries)
            .SelectMany(entry => entry.KeyWords)
            .ToList();

        IDictionary<string, string> queryParameters = new Dictionary<string, string>();

        IList<Item> nonCountableItems = queryStructure.Items.Where(item => !item.IsCountable).ToList();
        ConcurrentDictionary<Item, string> dictionary = new ConcurrentDictionary<Item, string>();
        
        Parallel.ForEach(nonCountableItems, item =>
        {
            dictionary.TryAdd(item, BuildParamForNonCountableItem(item, allNouns, words).Result);
        });
        
        dictionary.OrderBy(element => element.Key.Rank).ToList().ForEach(element =>
        {
            if (string.IsNullOrEmpty(element.Value)) return;
            string key = element.Value.Split('=')[0];
            queryParameters[key] = element.Value;
        });
        
        
        IList<Item> countableItems = queryStructure.Items.Where(item => item.IsCountable).ToList();
        ConcurrentDictionary<Item, List<WordInfo>> countableDictionary = new ConcurrentDictionary<Item, List<WordInfo>>();
        
        Parallel.ForEach(countableItems, item =>
        {
            countableDictionary.TryAdd(item, BuildParamForCountableItem(item, allNouns, words));
        });
        
        countableDictionary.OrderBy(element => element.Key.Rank).ToList().ForEach(element =>
        {
            if (element.Value.Count == 0) return;
            HashSet<string> values = FilterAndGetWordInfoValue(element.Value, element.Key, queryParameters);
            if (values.Count == 0) return;
            queryParameters[element.Key.Name] = $"{element.Key.Name}={string.Join(",", values)}";
        });
        
        return Task.FromResult(queryParameters.Count > 0 ? "?" + string.Join("&", queryParameters.Values) : string.Empty);
    }
    
    private static string AddSpaceBeforeFirstNumber(string input)
    {
        return BlankSpaces().Replace(input.Replace("$", "$ "), " ");
    }
    
    private static string AddSpaceAfterLastNumber(string rawSearchTerms)
    {
        string[] words = rawSearchTerms.Split(' ');
        if (words.Length == 0) return rawSearchTerms;

        StringBuilder builder = new StringBuilder();
        foreach (string word in words)
        {
            Match lastNumber = LastNumberOfNumericSequence().Match(word);
            if (lastNumber.Success)
            {
                builder.Append(word.AsSpan(0, lastNumber.Index + lastNumber.Length));
                builder.Append(' ');
                builder.Append(word.AsSpan(lastNumber.Index + lastNumber.Length));
                builder.Append(' ');
                continue;
            }

            builder.Append(word);
            builder.Append(' ');
        }
        
        return BlankSpaces().Replace(builder.ToString().Trim(), " ");
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
            
            if (ReplaceBySpace.Contains(textEnumerator.GetTextElement()))
            {
                stringBuilder.Append(' ');
                continue;
            }            
            if (ReplaceByEmpty.Contains(textEnumerator.GetTextElement()))
            {
                continue;
            }

            stringBuilder.Append(textEnumerator.GetTextElement());
        }

        string result = stringBuilder.ToString();

        return BlankSpaces().Replace(result, " ");
    }

    private static List<WordInfo> BuildParamForCountableItem(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<Entry> entries = item.Entries;

        return entries.Count == 0 ? Task.FromResult(new List<WordInfo>()).Result : BuildCountableItem(item, allNouns, words);
    }
    
    private static Task<string> BuildParamForNonCountableItem(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<Entry> entries = item.Entries;

        if (entries.Count == 0) return Task.FromResult(string.Empty);

        HashSet<string> values = BuildNonCountableItem(item, RephraseNumerals(item, allNouns, words));
        
        string result = values.Count > 0 ? $"{item.Name}={string.Join(",", values)}" : string.Empty;

        return Task.FromResult(result);
    }

    private static IList<string> RephraseNumerals(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        IList<string> recycleWords = wordInfos.Select(wordInfo => wordInfo.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()).ToList();

        return recycleWords;
    }

    private static List<WordInfo> BuildCountableItem(Item item, IList<string> allNouns, IList<string> words)
    {
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, allNouns, item.ConfirmationWords, item.RevocationWords);

        IList<string> itemKeyWords = item.Entries.SelectMany(element => element.KeyWords).ToList();
        
        return GroupWordInfos(wordInfos, itemKeyWords, item.WaitForConfirmationWords);
    }

    private static List<WordInfo> GroupWordInfos(IList<WordInfo> wordInfos, ICollection<string> keyWords, bool waitForConfirmation)
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

    private static void ProcessWordInfos(IList<WordInfo> wordInfos, ICollection<string> keyWords, bool waitForConfirmation, IList<WordInfo> wordInfoList, ICollection<WordInfo> candidates)
    {
        int lastPosition = wordInfos.Count - 1;

        for (int index = lastPosition; index >= 0; index--)
        {
            WordInfo wordInfo = wordInfos[index];

            if (HandleQuantifiedNoun(wordInfo, keyWords, candidates)) continue;
            if (wordInfo.Type == WordInfoType.Noun && index > 0 && wordInfos[index - 1].Type == WordInfoType.QuantifiedNoun) continue;
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

    private static bool IsIncompatible(Entry entry, IDictionary<string, string> queryParameters)
    {
        bool isIncompatible = false;

        foreach (KeyValuePair<string, string> parameter in queryParameters)
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

    private static HashSet<string> PickupMultipleEntryKeys(ICollection<Entry> entries, IList<string> words)
    {
        IDictionary<Entry, int> entrySimilarity = new Dictionary<Entry, int>(entries.Count);

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

        IDictionary<Entry, int> topEntrySimilarity = entrySimilarity.Where(keyValuePair => keyValuePair.Value >= 80)
            .OrderByDescending(keyValuePair => keyValuePair.Value)
            .ToDictionary(keyValuePair => keyValuePair.Key, pair => pair.Value);

        if (!topEntrySimilarity.Any()) return new HashSet<string>();

        // Get all entries with the Immiscible attribute
        IList<KeyValuePair<Entry, int>> immiscibleEntries = topEntrySimilarity.Where(keyValuePair
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
            // Otherwise, return all keys with the similarity greater than 80%
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