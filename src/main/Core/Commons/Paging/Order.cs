using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class Order
{
    public Direction Direction { get; init; }
    public string Property { get; init; }

    public Order(Direction direction, string property)
    {
        Direction = direction;
        Property = property;
    }

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