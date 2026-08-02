using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Domains.QueryStrings.UseCases;

// Turns the raw search terms typed by the user into the lower cased word list the resolvers work on.
//
// Punctuation is either dropped or turned into a separator, emoji are isolated so they can be matched
// as words of their own, and numbers glued to their unit ("100m2", "R$5000") are split apart.
public static partial class SearchTermsNormalizer
{
    private static readonly string[] ReplaceBySpace = [". ", ",00", "?", ";", "-", "(", ")", "[", "]", "{", "}", "\"", "/", "\\"];
    private static readonly string[] ReplaceByEmpty = ["."];
    private static readonly string[] Separators = [" ", ","];

    [GeneratedRegex("\\s+")]
    private static partial Regex BlankSpaces();

    [GeneratedRegex(@"\d+(?!\d)")]
    private static partial Regex LastNumberOfNumericSequence();

    public static IList<string> ToWords(string rawSearchTerms)
    {
        string searchTerms = AddSpaceBeforeFirstNumber(AddSpaceAfterLastNumber(Normalize(rawSearchTerms)));

        return [.. searchTerms.ToLower().Split(Separators, StringSplitOptions.RemoveEmptyEntries)];
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

    private static string Normalize(string rawSearchTerms)
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

        return BlankSpaces().Replace(stringBuilder.ToString(), " ");
    }
}
