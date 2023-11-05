using System.Threading.Tasks;
using Core.Domains.QueryStrings.Commands;
using Core.Domains.QueryStrings.Models;

namespace Core.Domains.QueryStrings.Interfaces;

public interface IBuildQueryStringUseCase
{
    Task<Output> ExecuteAsync(BuildQueryStringCommand command);
}