using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Interfaces;
using Core.Domains.QueryStructures.Models;
using Core.Exceptions;

namespace Core.Domains.QueryStructures.UseCases;

public sealed class AddQueryStructureItemUseCase : IAddQueryStructureItemUseCase
{
    private readonly IGetQueryStructureByIdGateway _getQueryStructureByIdGateway;
    private readonly IUpdateQueryStructureGateway _updateQueryStructureGateway;
    private readonly IGetDateTime _getDateTime;
    
    public AddQueryStructureItemUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _getQueryStructureByIdGateway = serviceResolver.Resolve<IGetQueryStructureByIdGateway>();
        _updateQueryStructureGateway = serviceResolver.Resolve<IUpdateQueryStructureGateway>();
        _getDateTime = serviceResolver.Resolve<IGetDateTime>();
    }
    
    public async Task<QueryStructure> ExecuteAsync(AddQueryStructureItemCommand command)
    {
        Optional<QueryStructure> optional = await _getQueryStructureByIdGateway.ExecuteAsync(command.QueryStructureId);
        
        QueryStructure queryStructure = optional.OrElseThrow(() => new NotFoundException("ValidationQueryStructureByIdNotFound", command.QueryStructureId));

        Item item = new Item
        {
            Name = command.Name,
            Description = command.Description,
            IsCountable = command.IsCountable,
            WaitForConfirmationWords = command.WaitForConfirmationWords,
            ConfirmationWords = command.ConfirmationWords,
            RevocationWords = command.RevocationWords
        };
        
        queryStructure.Items.Add(item);
        queryStructure.UpdatedAt = _getDateTime.Now();
        
        return await _updateQueryStructureGateway.ExecuteAsync(queryStructure);
    }
}