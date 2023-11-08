using System.Threading.Tasks;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Interfaces;

public interface IAddQueryStructureItemUseCase
{
    Task<QueryStructure> ExecuteAsync(AddQueryStructureItemCommand command);
}