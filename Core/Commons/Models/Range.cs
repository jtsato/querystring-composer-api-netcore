using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Commons.Models;

public readonly struct Range<T>
{
    public readonly T From;
    public readonly T To;

    private Range(T from, T to)
    {
        From = from;
        To = to;
    }

    public static Range<T> Of(T from, T to)
    {
        return new Range<T>(from, to);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(Range<T> other)
    {
        return Equals(From, other.From) && Equals(To, other.To);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return obj is Range<T> other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        unchecked
        {
            return ((From != null ? From.GetHashCode() : 0) * 397) ^ (To != null ? To.GetHashCode() : 0);
        }
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .Append($"{nameof(From)}: {From}, {nameof(To)}: {To}")
            .ToString();
    }

    public static bool operator ==(Range<T> left, Range<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Range<T> left, Range<T> right)
    {
        return !(left == right);
    }
}