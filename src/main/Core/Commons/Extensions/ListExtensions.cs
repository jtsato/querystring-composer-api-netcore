using System;
using System.Collections.Generic;

namespace Core.Commons.Extensions;

public static class ListExtensions
{
    /*
    Returns an empty list if the list is null.
     */
    public static IList<T> AsEmptyIfNull<T>(this IList<T> list)
    {
        return list ?? Array.Empty<T>();
    }
}