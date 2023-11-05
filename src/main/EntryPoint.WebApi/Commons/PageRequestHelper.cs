using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Commons.Extensions;
using Core.Commons.Paging;

namespace EntryPoint.WebApi.Commons;

public static partial class PageRequestHelper
{
    private const string FieldSeparator = ",";
    private const string DirectionSeparator = ":";

    [GeneratedRegex("\\s+")]
    private static partial Regex BlankSpaces();

    public static PageRequest Of(string rawPageNumber, string rawPageSize, string rawOrderBy)
    {
        int pageNumber = int.TryParse(rawPageNumber, out int intPageNumber) ? intPageNumber : 0;
        int pageSize = int.TryParse(rawPageSize, out int intPageSize) ? intPageSize : PageRequest.DefaultPageSize;
        return PageRequest.Of(pageNumber, pageSize, GetSort(rawOrderBy));
    }

    private static Sort GetSort(string rawOrderBy)
    {
        if (string.IsNullOrWhiteSpace(rawOrderBy)) return Sort.Unsorted;

        string orderBy = BlankSpaces().Replace(rawOrderBy, "");

        string[] strings = orderBy.Split(FieldSeparator);
        List<string> properties = strings
            .Select(it => it.SubstringBefore(DirectionSeparator)).ToList();

        return Sort.By(properties.Select(property => GetOrder(property, orderBy)).ToList());
    }

    private static Order GetOrder(string property, string orderBy)
    {
        Direction direction = GetDirection(property, orderBy);
        return new Order(direction, property);
    }

    private static Direction GetDirection(string property, string orderBy)
    {
        string direction = orderBy.SubstringAfter(property + DirectionSeparator).SubstringBefore(FieldSeparator);
        bool isDescending = string.Equals(direction, Convert.ToString(Direction.Desc), StringComparison.InvariantCultureIgnoreCase);
        return isDescending ? Direction.Desc : Direction.Asc;
    }
}