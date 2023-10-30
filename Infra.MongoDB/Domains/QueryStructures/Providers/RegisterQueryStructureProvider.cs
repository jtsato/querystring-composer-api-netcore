using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Mappers;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Providers;

public sealed class RegisterQueryStructureProvider : IRegisterQueryStructureGateway
{
    private const string KeyField = "id";

    private readonly IRepository<QueryStructureEntity> _entityRepository;
    private readonly ISequenceRepository<QueryStructureSequence> _sequenceRepository;
    
    public RegisterQueryStructureProvider(IRepository<QueryStructureEntity> entityRepository, ISequenceRepository<QueryStructureSequence> sequenceRepository)
    {
        _entityRepository = ArgumentValidator.CheckNull(entityRepository, nameof(entityRepository));
        _sequenceRepository = ArgumentValidator.CheckNull(sequenceRepository, nameof(sequenceRepository));
    }

    public async Task<QueryStructure> ExecuteAsync(QueryStructure queryStructure)
    {
        QueryStructureEntity entity = queryStructure.ToEntity();
        await IncrementId(entity);

        return (await _entityRepository.SaveAsync(entity)).ToModel();
    }

    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<QueryStructureSequence> filterDefinition = Builders<QueryStructureSequence>.Filter.Eq(sequence => sequence.SequenceName, KeyField);
        ISequence sequence = await _sequenceRepository.GetSequenceAndUpdate(filterDefinition);
        entity.Id = sequence.SequenceValue;
    }
}
