using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class Pageable
{
    public int Page { get; }
    public int Size { get; }
    public int NumberOfElements { get; }
    public long TotalOfElements { get; }
    public int TotalPages { get; }

    public Pageable(int page, int size, int numberOfElements, long totalOfElements, int totalPages)
    {
        Page = page;
        Size = size;
        NumberOfElements = numberOfElements;
        TotalOfElements = totalOfElements;
        TotalPages = totalPages;
    }
}