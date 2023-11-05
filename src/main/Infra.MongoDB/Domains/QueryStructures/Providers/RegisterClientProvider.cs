using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Clients.Gateways;
using Core.Domains.Clients.Models;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Mappers;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Providers;

public class RegisterClientProvider : IRegisterClientGateway
{
    private const string KeyField = "id";

    private readonly IRepository<ClientEntity> _entityRepository;
    private readonly ISequenceRepository<ClientSequence> _sequenceRepository;

    public RegisterClientProvider(IRepository<ClientEntity> clientRepository, ISequenceRepository<ClientSequence> clientSequenceRepository)
    {
        _entityRepository = ArgumentValidator.CheckNull(clientRepository, nameof(clientRepository));
        _sequenceRepository = ArgumentValidator.CheckNull(clientSequenceRepository, nameof(clientSequenceRepository));
    }

    public async Task<Client> ExecuteAsync(Client client)
    {
        ClientEntity entity = client.ToEntity();
        await IncrementId(entity);

        return (await _entityRepository.SaveAsync(entity)).ToModel();
    }

    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<ClientSequence> filter = Builders<ClientSequence>.Filter.Eq(sequence => sequence.SequenceName, KeyField);
        ISequence sequence = await _sequenceRepository.GetSequenceAndUpdate(filter);
        entity.Id = sequence.SequenceValue;
    }
}
