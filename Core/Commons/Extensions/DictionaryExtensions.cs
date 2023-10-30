using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Commons.Extensions;

public static class DictionaryExtensions
{
    private static readonly Dictionary<object, object> EmptyObjectDictionary = new Dictionary<object, object>(0);

    private static Dictionary<TKey, TValue> GetEmptyDictionary<TKey, TValue>()
    {
        return EmptyObjectDictionary as Dictionary<TKey, TValue> ?? new Dictionary<TKey, TValue>();
    }

    public static IReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>()
    {
        return new ReadOnlyDictionary<TKey, TValue>(GetEmptyDictionary<TKey, TValue>());
    }

    /*
    Returns an empty dictionary if the dictionary is null.
     */
    public static IDictionary<TKey, TValue> AsEmptyIfNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        return dictionary ?? new Dictionary<TKey, TValue>();
    }
}
