using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Mappers;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Providers;

public class GetQueryStructureByNameProvider : IGetQueryStructureByNameGateway
{
    private readonly IRepository<QueryStructureEntity> _clientRepository;

    public GetQueryStructureByNameProvider(IRepository<QueryStructureEntity> clientRepository)
    {
        _clientRepository = ArgumentValidator.CheckNull(clientRepository, nameof(clientRepository));
    }

    public async Task<Core.Commons.Optional<QueryStructure>> ExecuteAsync(string clientUid, string name)
    {
        FilterDefinition<QueryStructureEntity> filterDefinitions = GetFilterDefinition(clientUid, name);
        Core.Commons.Optional<QueryStructureEntity> optional = await _clientRepository.FindOneAsync(filterDefinitions);

        return optional.Map(QueryStructureMapper.ToModel);
    }

    private static FilterDefinition<QueryStructureEntity> GetFilterDefinition(string clientUid, string name)
    {
        IList<FilterDefinition<QueryStructureEntity>> filters = new List<FilterDefinition<QueryStructureEntity>>();

        FilterHelper.AddEqualsFilter(filters, entity => entity.ClientUid, clientUid);
        FilterHelper.AddEqualsFilter(filters, entity => entity.Name, name);

        return filters.Count == 0 ? Builders<QueryStructureEntity>.Filter.Empty : Builders<QueryStructureEntity>.Filter.And(filters);
    }
}
