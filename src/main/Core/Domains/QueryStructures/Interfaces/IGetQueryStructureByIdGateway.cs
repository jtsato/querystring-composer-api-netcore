using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Interfaces;

public interface IGetQueryStructureByIdGateway
{
    Task<Optional<QueryStructure>> ExecuteAsync(int queryStructureId);
}