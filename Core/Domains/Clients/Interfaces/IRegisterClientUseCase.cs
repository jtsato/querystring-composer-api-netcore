using System.Threading.Tasks;
using Core.Domains.Clients.Commands;
using Core.Domains.Clients.Models;

namespace Core.Domains.Clients.Interfaces;

public interface IRegisterClientUseCase
{
    Task<Client> ExecuteAsync(RegisterClientCommand command);
}