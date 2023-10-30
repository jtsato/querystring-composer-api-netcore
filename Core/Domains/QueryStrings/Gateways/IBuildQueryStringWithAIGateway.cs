using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStrings.Gateways;

public interface IBuildQueryStringWithAiGateway
{
    Task<Optional<string>> ExecuteAsync(QueryStructure queryStructure, string searchTerm);
}