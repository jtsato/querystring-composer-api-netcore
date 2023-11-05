using System;
using System.Globalization;

namespace Core.Commons.Extensions;

public static class StringExtensions
{
    private const int IndexNotFound = -1;

    /*
    Gets the substring before the first occurrence of a separator.
    The separator is not returned.
    A null string input will return null.
    An empty ("") string input will return the empty string.
    A null separator will return the input string.
    If nothing is found, the string input is returned.
    */
    public static string SubstringBefore(this string text, string stopAt)
    {
        if (text is null) return null;
        if (stopAt is null) return text;
        int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);
        return charLocation == IndexNotFound ? text : text[..charLocation];
    }

    /*
    Gets the substring before the last occurrence of a separator.
    The separator is not returned.
    A null string input will return null.
    An empty ("") string input will return the empty string.
    A null separator will return the input string.
    If nothing is found, the string input is returned.
    */
    public static string SubstringBeforeLast(this string text, string stopAt)
    {
        if (text is null) return null;
        if (stopAt is null) return text;
        int charLocation = text.LastIndexOf(stopAt, StringComparison.Ordinal);
        return charLocation == IndexNotFound ? text : text[..charLocation];
    }

    /*
    Gets the substring after the first occurrence of a separator.
    The separator is not returned.
    A null string input will return null.
    An empty ("") string input will return the empty string.
    If nothing is found, the empty string is returned.
    */
    public static string SubstringAfter(this string text, string startAt)
    {
        if (string.IsNullOrEmpty(text) || (string.IsNullOrEmpty(startAt))) return text;
        int charLocation = text.IndexOf(startAt, StringComparison.Ordinal);
        return charLocation == IndexNotFound ? string.Empty : text[(charLocation + startAt.Length)..];
    }

    /*
    Gets the substring after the last occurrence of a separator.
    The separator is not returned.
    A null string input will return null.
    An empty ("") string input will return the empty string.
    If nothing is found, the empty string is returned.
    */
    public static string SubstringAfterLast(this string text, string startAt)
    {
        if (string.IsNullOrEmpty(text) || (string.IsNullOrEmpty(startAt))) return text;
        int charLocation = text.LastIndexOf(startAt, StringComparison.Ordinal);
        return charLocation == IndexNotFound ? string.Empty : text[(charLocation + startAt.Length)..];
    }

    /**
        Appends the suffix to the end of the string if the string does not already end with the suffix.
        A null string input will return null.
        An empty ("") string input will return the empty string.
        The ignoreCase parameters indicates whether the compare should ignore case.
        Returns a new String if suffix was appended, the same string otherwise.
        */
    public static string AppendIfMissing(this string text, string suffix, bool ignoreCase = false)
    {
        if (text == null || string.IsNullOrEmpty(suffix) || text.EndsWith(suffix, ignoreCase, CultureInfo.InvariantCulture))
        {
            return text;
        }

        return $"{text}{suffix}";
    }

    /**
        Truncates a String.
        This will turn "Now is the time for all good men" into "Now is the time for".
        If text is less than maxWidth characters long, return it.
        Else truncate it to text.substring(0, maxWidth).
        If maxWidth is less than 0, throw an IllegalArgumentException.
        In no case will it return a string of length greater than maxWidth.
     */
    public static string Truncate(this string text, int maxLength)
    {
        if (maxLength < 0)
        {
            throw new ArgumentException("Max width must be greater than or equal to zero.");
        }

        if (text is null || text.Length <= maxLength)
        {
            return text;
        }

        return text[..maxLength];
    }
}