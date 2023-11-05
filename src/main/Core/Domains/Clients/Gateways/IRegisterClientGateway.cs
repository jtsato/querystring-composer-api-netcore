using System.Threading.Tasks;
using Core.Domains.Clients.Models;

namespace Core.Domains.Clients.Gateways;

public interface IRegisterClientGateway
{
    Task<Client> ExecuteAsync(Client client);
}
