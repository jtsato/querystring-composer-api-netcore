using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Schema;

namespace Core.Commons;

public static class ArgumentChecker
{
    public static bool IsPercentage(string input)
    {
        return string.IsNullOrEmpty(input) || byte.TryParse(input, out byte percentage) && percentage <= 100;
    }
    
    public static bool IsByte(string input)
    {
        return string.IsNullOrEmpty(input) || byte.TryParse(input, out _);
    }
    
    public static bool IsInteger(string input)
    {
        return string.IsNullOrEmpty(input) || int.TryParse(input, out _);
    }

    public static bool IsLong(string input)
    {
        return string.IsNullOrEmpty(input) || long.TryParse(input, out _);
    }

    public static bool IsFloat(string input)
    {
        return string.IsNullOrEmpty(input) || float.TryParse(input, out _);
    }

    public static bool IsDouble(string input)
    {
        return string.IsNullOrEmpty(input) || double.TryParse(input, out _);
    }

    public static bool IsValidUri(string link)
    {
        return Uri.TryCreate(link, UriKind.Absolute, out Uri outUri)
               && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
    }

    public static bool HasDuplicatedValues<T>(IEnumerable<T> elements)
    {
        IEnumerable<T> duplicates = elements.GroupBy(element => element)
            .Where(element => element.Count() > 1)
            .Select(element => element.Key);
        return duplicates.Any();
    }

    public static bool IsValidEnumOf<T>(string candidate) where T : Enumeration<T>
    {
        return Enumeration<T>.GetByName(candidate).HasValue();
    }

    public static bool BeEmptyOrAValidDate(string dateTimeIso8601)
    {
        IFormatProvider culture = CultureInfo.InvariantCulture;
        const DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;
        return string.IsNullOrWhiteSpace(dateTimeIso8601) || DateTime.TryParse(dateTimeIso8601, culture, styles, out _);
    }

    public static bool IsJson(string input)
    {
        try
        {
            JsonDocument.Parse(input);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool HaveAllRequiredSchemaFields(string input)
    {
        try
        {
            JSchema schema = JSchema.Parse(input);

            return schema.Properties.Count > 0 && schema.Required.Count > 0 && schema.Type.HasValue;
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    public static bool HaveAllPropertiesTyped(string input)
    {
        try
        {
            JSchema schema = JSchema.Parse(input);

            return SchemaHasAllPropertiesTyped(schema.Properties);
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    private static bool SchemaHasAllPropertiesTyped(IDictionary<string, JSchema> properties)
    {
        foreach ((_, JSchema value) in properties.Select(property => (property.Key, property.Value)))
        {
            if (!value.Type.HasValue) return false;
            if (!SchemaHasAllPropertiesTyped(value.Properties)) return false;
        }

        return true;
    }
}