using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Helpers;

public static class UpdateHelper
{
    public static void AddUpDefinitionIfValueHasChanged<T, TU>
    (
        ref List<UpdateDefinition<TU>> definitions,
        string fieldName,
        T currentValue,
        T newValue
    )
    {
        if (currentValue is not null && CompareItem(currentValue, newValue)) return;
        definitions.Add(Builders<TU>.Update.Set(fieldName, newValue));
    }

    public static void AddUpDefinitionIfItemsHasChanged<T, TU>
    (
        ref List<UpdateDefinition<TU>> definitions,
        string fieldName,
        List<T> currentValue,
        List<T> newValue
    )
    {
        if (CompareList(currentValue, newValue)) return;
        definitions.Add(Builders<TU>.Update.Set(fieldName, newValue));
    }

    private static bool CompareItem<T>(T first, T second)
    {
        return EqualityComparer<T>.Default.Equals(first, second);
    }

    private static bool CompareList<T>(List<T> first, List<T> second)
    {
        if (first is null && second is null) return true;
        if (first is null || second is null) return false;

        return !first.Except(second).Any() && !second.Except(first).Any();
    }
}