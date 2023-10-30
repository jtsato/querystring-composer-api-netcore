using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Commons;
using Core.Domains.QueryStrings.Models;

namespace Core.Domains.QueryStrings.Helpers;

public static class SentenceParserHelper
{
    private const double SimilarityLimit = 0.8;

    private const string Anonymous = "Anonymous";
    private const string DefaultConjunction = "e";
    private const string DefaultIntervalPreposition = "a";

    private static readonly CultureInfo DefaultCultureInfo = new CultureInfo("pt-BR");

    private static readonly List<long> ScaleSeparators = new List<long> {1000, 1000000, 1000000000};

    public static IList<WordInfo> Parse(IList<string> words, IList<string> nouns, IList<string> confirmationWords, IList<string> revocationWords)
    {
        IList<WordInfo> wordInfos = Classify(words, nouns, confirmationWords, revocationWords);

        return Summarize(wordInfos, confirmationWords, revocationWords).ToArray();
    }

    private static IList<WordInfo> Classify(IList<string> words, IList<string> nouns, IList<string> confirmationWords, IList<string> revocationWords)
    {
        List<WordInfo> wordInfos = new List<WordInfo>();

        foreach (string current in words)
        {
            WordInfo info = new WordInfo
            {
                Value = current
            };
            
            if (int.TryParse(current, NumberStyles.Number, DefaultCultureInfo, out int result))
            {
                info.Type = WordInfoType.QuantitativeAdjective;
                info.Value = Convert.ToString(result, DefaultCultureInfo);
                wordInfos.Add(info);
                continue;
            }

            long number = NumberConverterHelper.ConvertWordsToNumber(current);
            if (number > 0)
            {
                info.Type = WordInfoType.QuantitativeAdjective;
                info.Value = Convert.ToString(number, DefaultCultureInfo);
                wordInfos.Add(info);
                continue;
            }

            (double maxSimilarity, string mostSimilarWord) = GetMaxSimilarity(nouns, current);
            if (maxSimilarity >= SimilarityLimit)
            {
                info.Type = WordInfoType.Noun;
                info.Value = mostSimilarWord;
                wordInfos.Add(info);
                continue;
            }

            (maxSimilarity, mostSimilarWord) = GetMaxSimilarity(confirmationWords, current);
            if (maxSimilarity >= SimilarityLimit)
            {
                info.Type = WordInfoType.ConfirmationIndicator;
                info.Value = mostSimilarWord;
                wordInfos.Add(info);
                continue;
            }

            (maxSimilarity, mostSimilarWord) = GetMaxSimilarity(revocationWords, current);
            if (maxSimilarity >= SimilarityLimit)
            {
                info.Type = WordInfoType.RevocationIndicator;
                info.Value = mostSimilarWord;
                wordInfos.Add(info);
                continue;
            }

            info.Type = WordInfoType.Other;
            wordInfos.Add(info);
        }

        return wordInfos;
    }

    private static (double maxSimilarity, string mostSimilarWord) GetMaxSimilarity(IEnumerable<string> words, string term)
    {
        double maxSimilarity = 0;
        string mostSimilarWord = string.Empty;

        foreach (string word in words)
        {
            double similarity = GetSimilarityHelper.GetSimilarity(word, term);
            if (similarity > maxSimilarity)
            {
                maxSimilarity = similarity;
                mostSimilarWord = word;
            }

            if (maxSimilarity.CompareTo(1) == 0) break;
        }

        return (maxSimilarity, mostSimilarWord);
    }

    private static List<WordInfo> Summarize(IList<WordInfo> wordInfos, IList<string> confirmationWords, IList<string> revocationWords)
    {
        HashSet<string> indicators = new HashSet<string>(confirmationWords);
        indicators.UnionWith(revocationWords);

        List<WordInfo> summarize = new List<WordInfo>();

        State state = new State();

        for (int index = 0; index < wordInfos.Count; index++)
        {
            WordInfo wordInfo = wordInfos[index];

            if (wordInfo.Type != WordInfoType.QuantitativeAdjective)
            {
                bool isPreposition = indicators.TryGetValue(wordInfo.Value, out string _);
                HandleOtherWordInfos(wordInfos, state, index, isPreposition, confirmationWords, summarize);
                continue;
            }

            HandleQuantitativeAdjective(wordInfos, state, index, confirmationWords, revocationWords, summarize);
        }

        state.TotalValue = state.TotalValue > 0 ? state.TotalValue : state.CurrentValue;

        if (state.TotalValue <= 0) return summarize;

        AddQuantifiedNoun(summarize, confirmationWords, state.TotalValue, Anonymous);

        return summarize;
    }

    private static void HandleOtherWordInfos(IList<WordInfo> wordInfos, State state, int index, bool isPreposition, IList<string> confirmationWords, List<WordInfo> summarize)
    {
        WordInfo wordInfo = wordInfos[index];

        bool hasNext = index <= wordInfos.Count - 2;
        bool runTheTotalizator = isPreposition || wordInfo.Type.Is(WordInfoType.Noun) || !hasNext ||
                                 wordInfos[index + 1].Type.IsNot(WordInfoType.QuantitativeAdjective);

        if (!runTheTotalizator)
        {
            if (wordInfo.Value != DefaultConjunction)
            {
                summarize.Add(wordInfo);
            }

            return;
        }

        state.TotalValue = state.TotalValue > 0 ? state.TotalValue : state.CurrentValue;

        if (state.TotalValue > 0)
        {
            string noun = ComputeNoun(wordInfos, index);
            AddQuantifiedNoun(summarize, confirmationWords, state.TotalValue, noun);
            ResetState(state);
        }

        summarize.Add(wordInfo);
    }

    private static void HandleQuantitativeAdjective(IList<WordInfo> wordInfos, State state, int index, IList<string> confirmationWords,
        IList<string> revocationWords, List<WordInfo> summarize)
    {
        WordInfo wordInfo = wordInfos[index];

        string value = wordInfo.Value;
        long number = Convert.ToInt64(value, DefaultCultureInfo);

        if (!ScaleSeparators.Contains(number))
        {
            state.CurrentValue += number;
            return;
        }

        if (index == 0)
        {
            state.TotalValue += (state.CurrentValue == 0 ? 1 : state.CurrentValue) * number;
            state.CurrentValue = 0;
            state.CurrentScale = NumberScale.GetScaleByNumber(state.TotalValue);
            return;
        }

        state.CurrentValue = state.CurrentValue == 0 ? number : state.CurrentValue * number;

        NumberScale scale = NumberScale.GetScaleByNumber(state.CurrentValue);

        if (state.CurrentScale.Id != NumberScale.None.Id && scale.Id >= state.CurrentScale.Id)
        {
            string noun = ComputeNoun(wordInfos, index);

            AddQuantifiedNoun(summarize, confirmationWords, state.TotalValue, noun);
            AddIndicator(summarize, confirmationWords, revocationWords);

            state.TotalValue = state.CurrentValue;
            state.CurrentValue = 0;
            state.CurrentScale = scale;
            return;
        }

        state.TotalValue += state.CurrentValue;
        state.CurrentValue = 0;
        state.CurrentScale = scale;
    }

    private static void ResetState(State state)
    {
        state.TotalValue = 0;
        state.CurrentValue = 0;
        state.CurrentScale = NumberScale.None;
    }

    private static string ComputeNoun(IList<WordInfo> wordInfos, int index)
    {
        for (int zIndex = index; zIndex < wordInfos.Count; zIndex++)
        {
            WordInfo wordInfo = wordInfos[zIndex];
            if (wordInfo.Type.Is(WordInfoType.Other) && wordInfo.Value != DefaultConjunction) return Anonymous;
            if (wordInfo.Type.Is(WordInfoType.Noun)) return wordInfo.Value;
        }

        return Anonymous;
    }

    private static void AddQuantifiedNoun(List<WordInfo> target, IList<string> confirmationWords, long totalValue, string noun)
    {
        Optional<WordInfo> optional = GetLastQuantifiedNoun(target);

        if (optional.HasValue())
        {
            WordInfo wordInfo = optional.GetValue();
            string value = wordInfo.Value.Split(' ')[1];
            double similarity = GetSimilarityHelper.GetSimilarity(value, noun);
            if (similarity >= 0.51)
            {
                WordInfoType type = confirmationWords.Contains(DefaultIntervalPreposition)
                    ? WordInfoType.ConfirmationIndicator
                    : WordInfoType.RevocationIndicator;

                target.Add(new WordInfo
                {
                    Type = type,
                    Value = DefaultIntervalPreposition
                });
            }
        }

        target.Add(new WordInfo
        {
            Type = WordInfoType.QuantifiedNoun,
            Value = $"{totalValue} {noun}"
        });
    }

    private static Optional<WordInfo> GetLastQuantifiedNoun(IList<WordInfo> wordInfos)
    {
        if (wordInfos.Count == 0) return Optional<WordInfo>.Empty();

        for (int i = wordInfos.Count - 1; i >= 0; i--)
        {
            WordInfo wordInfo = wordInfos[i];
            if (wordInfo.Type.Is(WordInfoType.QuantifiedNoun)) return Optional<WordInfo>.Of(wordInfo);
            if (wordInfo.Type.Is(WordInfoType.RevocationIndicator)) return Optional<WordInfo>.Empty();
            if (wordInfo.Type.Is(WordInfoType.ConfirmationIndicator)) return Optional<WordInfo>.Empty();
        }

        return Optional<WordInfo>.Empty();
    }

    private static void AddIndicator(List<WordInfo> target, IList<string> confirmationWords, IList<string> revocationWords)
    {
        WordInfoType type = GetWordInfoType(confirmationWords, revocationWords);

        target.Add(new WordInfo
        {
            Type = type,
            Value = DefaultIntervalPreposition
        });
    }

    private static WordInfoType GetWordInfoType(IList<string> confirmationWords, IList<string> revocationWords)
    {
        bool isConfirmation = confirmationWords.Contains(DefaultIntervalPreposition);
        if (isConfirmation) return WordInfoType.ConfirmationIndicator;

        bool isRevocation = revocationWords.Contains(DefaultIntervalPreposition);

        return isRevocation ? WordInfoType.RevocationIndicator : WordInfoType.Other;
    }

    private sealed class State
    {
        public NumberScale CurrentScale = NumberScale.None;
        public long TotalValue { get; set; }
        public long CurrentValue { get; set; }
    }
}