using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Clients.Commands;
using Core.Domains.Clients.Gateways;
using Core.Domains.Clients.Interfaces;
using Core.Domains.Clients.Models;

namespace Core.Domains.Clients.UseCases;

public sealed class RegisterClientUseCase : IRegisterClientUseCase
{
    private readonly IRegisterClientGateway _registerClientGateway;
    private readonly IGetDateTime _getDateTime;
    
    public RegisterClientUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _registerClientGateway = serviceResolver.Resolve<IRegisterClientGateway>();
        _getDateTime = serviceResolver.Resolve<IGetDateTime>();
    }
    
    public async Task<Client> ExecuteAsync(RegisterClientCommand command)
    {
        Client client = new Client
        {
            Uid = command.Uid,
            Name = command.Name,
            Description = command.Description,
            CreatedAt = _getDateTime.Now(),
            UpdatedAt = _getDateTime.Now()
        };
        
        return await _registerClientGateway.ExecuteAsync(client);
    }
}
