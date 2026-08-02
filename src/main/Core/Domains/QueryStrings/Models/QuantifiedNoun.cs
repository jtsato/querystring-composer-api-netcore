using System;
using System.Globalization;

namespace Core.Domains.QueryStrings.Models;

// A noun together with how many of it were asked for, for example 3 "quartos".
//
// The sentence parser encodes this as the string "3 quartos" inside WordInfo.Value. This type owns
// that encoding so the rest of the builder never has to Split(' ') and index into the halves.
public readonly struct QuantifiedNoun
{
    private const char Separator = ' ';

    public long Count { get; }
    public string Noun { get; }

    private QuantifiedNoun(long count, string noun)
    {
        Count = count;
        Noun = noun;
    }

    public static QuantifiedNoun Of(long count, string noun)
    {
        return new QuantifiedNoun(count, noun);
    }

    public static QuantifiedNoun Parse(string value)
    {
        string[] parts = value.Split(Separator);

        return new QuantifiedNoun(long.Parse(parts[0], CultureInfo.InvariantCulture), parts[1]);
    }

    public QuantifiedNoun Increment()
    {
        return new QuantifiedNoun(Count + 1, Noun);
    }

    public string CountAsText => Count.ToString(CultureInfo.InvariantCulture);

    public override string ToString()
    {
        return $"{CountAsText}{Separator}{Noun}";
    }

    public override bool Equals(object obj)
    {
        return obj is QuantifiedNoun other && Count == other.Count && Noun == other.Noun;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Count, Noun);
    }

    public static bool operator ==(QuantifiedNoun left, QuantifiedNoun right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(QuantifiedNoun left, QuantifiedNoun right)
    {
        return !(left == right);
    }
}
