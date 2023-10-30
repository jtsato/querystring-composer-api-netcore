using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class Sort
{
    private const Direction DefaultDirection = Direction.Asc;
    public static readonly Sort Unsorted = By(Array.Empty<Order>());
    private readonly IReadOnlyCollection<Order> _orders;

    private Sort(IReadOnlyCollection<Order> orders)
    {
        _orders = orders;
    }

    private Sort(Direction direction, IReadOnlyCollection<string> properties)
    {
        ArgumentValidator.CheckNullOrEmpty(properties, nameof(properties), "You have to provide at least one property to sort by!");

        _orders = properties.Select(it => new Order(direction, it)).ToList();
    }

    public static Sort By(params string[] properties)
    {
        ArgumentValidator.CheckNull(properties, nameof(properties), "Properties must not be null!");

        return new Sort(DefaultDirection, properties);
    }

    private static Sort By(params Order[] orders)
    {
        ArgumentValidator.CheckNull(orders, nameof(orders), "Orders must not be null!");

        return new Sort(orders);
    }

    public static Sort By(IReadOnlyCollection<Order> orders)
    {
        ArgumentValidator.CheckNullOrEmpty(orders, nameof(orders), "Orders must not be null!");

        return By(orders.ToArray());
    }

    public IEnumerable<Order> GetOrders()
    {
        return _orders;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(Sort other)
    {
        return _orders.Count == other._orders.Count
               && !_orders.Except(other._orders).Any()
               && !other._orders.Except(_orders).Any();
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Sort other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return _orders is not null ? _orders.GetHashCode() : 0;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(_orders)}: {_orders}";
    }
}