using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public sealed class Pageable(int page, int size, int numberOfElements, long totalOfElements, int totalPages)
{
    public int Page { get; } = page;
    public int Size { get; } = size;
    public int NumberOfElements { get; } = numberOfElements;
    public long TotalOfElements { get; } = totalOfElements;
    public int TotalPages { get; } = totalPages;
}