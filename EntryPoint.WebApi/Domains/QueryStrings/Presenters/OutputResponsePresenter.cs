using Core.Domains.QueryStrings.Models;
using EntryPoint.WebApi.Domains.QueryStrings.Models;

namespace EntryPoint.WebApi.Domains.QueryStrings.Presenters;

public static class OutputResponsePresenter
{
    public static OutputResponse Map(Output output)
    {
        return new OutputResponse
        {
            ClientUid = output.ClientUid,
            QueryName = output.QueryName,
            SearchTerms = output.SearchTerms,
            QueryString = output.QueryString,
            CreatedByAi = output.CreatedByAi,
            CreatedAt = output.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
        };
    }
}
