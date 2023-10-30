using System.Threading.Tasks;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.Interfaces;

public interface IRegisterQueryStructureUseCase
{
    Task<QueryStructure> ExecuteAsync(RegisterQueryStructureCommand command);
}