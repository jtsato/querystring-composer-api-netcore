using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Paging;

[ExcludeFromCodeCoverage]
public class Page<T>
{
    public IReadOnlyList<T> Content { get; }
    public Pageable Pageable { get; }

    public Page(IReadOnlyList<T> content, Pageable pageable)
    {
        Content = content;
        Pageable = pageable;
    }
}