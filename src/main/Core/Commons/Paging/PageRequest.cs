using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class PageRequest
{
    public static int DefaultPageSize => 10;
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public Sort Sort { get; init; }

    private PageRequest(int pageNumber, int pageSize, Sort sort)
    {
        ArgumentValidator.CheckNegative(pageNumber, nameof(pageNumber), "Page index must not be less than zero!");
        ArgumentValidator.CheckNegativeOrZero(pageSize, nameof(pageSize), "Page size must not be less than one!");
        ArgumentValidator.CheckNull(sort, nameof(sort), "Sort must not be null!");

        PageNumber = pageNumber;
        PageSize = pageSize;
        Sort = sort;
    }

    public static PageRequest Of(int pageNumber, int pageSize)
    {
        return new PageRequest(pageNumber, pageSize, Sort.Unsorted);
    }

    public static PageRequest Of(int pageNumber, int pageSize, Sort sort)
    {
        return new PageRequest(pageNumber, pageSize, sort);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(PageRequest other)
    {
        return PageNumber == other.PageNumber
               && PageSize == other.PageSize
               && Equals(Sort, other.Sort);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PageRequest other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(PageNumber, PageSize, Sort);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(PageNumber)}: {PageNumber}, {nameof(PageSize)}: {PageSize}, {nameof(Sort)}: {Sort}";
    }
}