using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EntryPoint.WebApi.Commons;

public static partial class OrderByHelper
{
    private const string AscendingDirection = "ASC";
    private const string DescendingDirection = "DESC";
    private static readonly char[] Separators = {',', ':'};

    [GeneratedRegex("\\s+")]
    private static partial Regex BlankSpaces();

    public static string Sanitize(string[] sortableFields, List<string> rawOrderBys)
    {
        return rawOrderBys != null ? Sanitize(sortableFields, string.Join(",", rawOrderBys)) : string.Empty;
    }

    public static string Sanitize(string[] sortableFields, string rawOrderBy)
    {
        if (string.IsNullOrWhiteSpace(rawOrderBy)) return string.Empty;

        string orderBy = BlankSpaces().Replace(rawOrderBy, "");
        string[] values = orderBy.Split(Separators);
        List<string> sanitized = new List<string>();

        foreach (string value in values)
        {
            AddDirection(sanitized, IsDirectionCommand(value) ? value : AscendingDirection);
            if (sortableFields == null) continue;
            foreach (string field in sortableFields)
            {
                if (!field.Equals(value, StringComparison.InvariantCultureIgnoreCase)) continue;
                sanitized.Add(field);
                break;
            }
        }

        AddDirection(sanitized, AscendingDirection);

        return string.Join(",", sanitized.Where(it => !string.IsNullOrEmpty(it)));
    }

    private static void AddDirection(IList<string> sanitized, string value)
    {
        if (sanitized.Count <= 0 || IsDirectionCommand(sanitized[^1])) return;
        sanitized[^1] = string.Concat(sanitized[^1], ":", value.ToUpper());
    }

    private static bool IsDirectionCommand(string value)
    {
        return DescendingDirection.Equals(value, StringComparison.InvariantCultureIgnoreCase) ||
               value!.EndsWith($":{AscendingDirection}", StringComparison.InvariantCultureIgnoreCase) ||
               value!.EndsWith($":{DescendingDirection}", StringComparison.InvariantCultureIgnoreCase);
    }
}