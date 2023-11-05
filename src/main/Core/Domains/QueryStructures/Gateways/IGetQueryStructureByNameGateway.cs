using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Gateways;

public interface IGetQueryStructureByNameGateway
{
    Task<Optional<QueryStructure>> ExecuteAsync(string clientUid, string name);
}
