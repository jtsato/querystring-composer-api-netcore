using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Helpers;

public static class FilterHelper
{
    public static void AddEqualsFilter<T>(ICollection<FilterDefinition<T>> definitions, Expression<Func<T, object>> expression, int value)
    {
        if (value == 0) return;
        definitions.Add(Builders<T>.Filter.Eq(expression, value));
    }

    public static void AddEqualsFilter<T>(ICollection<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        filterDefinitions.Add(Builders<T>.Filter.Eq(expression, value));
    }

    public static void AddLikeFilter<T>(ICollection<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        filterDefinitions.Add(Builders<T>.Filter.Regex(expression, value));
    }

    public static void AddDateAfterOrEqualFilter<T>(ICollection<FilterDefinition<T>> definitions, Expression<Func<T, object>> expression, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        DateTime dateTime = DateTime.Parse(value, CultureInfo.DefaultThreadCurrentCulture).ToUniversalTime();
        definitions.Add(Builders<T>.Filter.Gte(expression, dateTime));
    }

    public static void AddDateBeforeOrEqualFilter<T>(ICollection<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        DateTime dateTime = DateTime.Parse(value, CultureInfo.DefaultThreadCurrentCulture).ToUniversalTime();
        filterDefinitions.Add(Builders<T>.Filter.Lte(expression, dateTime));
    }

    public static void AddGreaterOrEqualFilter<T>(List<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, float value)
    {
        if (value == 0) return;
        filterDefinitions.Add(Builders<T>.Filter.Gte(expression, value));
    }

    public static void AddLessOrEqualFilter<T>(List<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, float value)
    {
        if (value == 0) return;
        filterDefinitions.Add(Builders<T>.Filter.Lte(expression, value));
    }

    public static void AddInArrayFilter<T>(List<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, List<string> values)
    {
        if (values == null || values.Count == 0) return;
        filterDefinitions.Add(Builders<T>.Filter.In(expression, values));
    }
}