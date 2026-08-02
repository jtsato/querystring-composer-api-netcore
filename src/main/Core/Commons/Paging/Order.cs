using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class Order(Direction direction, string property)
{
    public Direction Direction { get; init; } = direction;
    public string Property { get; init; } = property;

    [ExcludeFromCodeCoverage]
    private bool Equals(Order other)
    {
        return Direction == other.Direction
               && Property == other.Property;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Order other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine((int) Direction, Property);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(Direction)}: {Direction}, {nameof(Property)}: {Property}";
    }
}