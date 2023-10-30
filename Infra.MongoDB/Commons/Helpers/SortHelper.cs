using System.Collections.Generic;
using System.Linq;
using Core.Commons.Paging;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Helpers;

public static class SortHelper
{
    public static SortDefinition<T> GetSortDefinitions<T>(IEnumerable<Order> orders)
    {
        List<SortDefinition<T>> definitions = orders
            .Select(order => order.Direction == Direction.Asc
                ? Builders<T>.Sort.Ascending(order.Property)
                : Builders<T>.Sort.Descending(order.Property))
            .ToList();

        return Builders<T>.Sort.Combine(definitions);
    }
}