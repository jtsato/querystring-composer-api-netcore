using System.Threading.Tasks;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Interfaces;

public interface IUpdateQueryStructureGateway
{
    Task<QueryStructure> ExecuteAsync(QueryStructure queryStructure);
}