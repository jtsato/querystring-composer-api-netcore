using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public class Page<T>(IReadOnlyList<T> content, Pageable pageable)
{
    public IReadOnlyList<T> Content { get; } = content;
    public Pageable Pageable { get; } = pageable;

}