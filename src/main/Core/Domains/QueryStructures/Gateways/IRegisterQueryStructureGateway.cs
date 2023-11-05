using System.Threading.Tasks;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Gateways;

public interface IRegisterQueryStructureGateway
{
    Task<QueryStructure> ExecuteAsync(QueryStructure queryStructure);
}