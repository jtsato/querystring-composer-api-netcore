using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Interfaces;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Mappers;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;
using static Infra.MongoDB.Commons.Helpers.UpdateHelper;

namespace Infra.MongoDB.Domains.QueryStructures.Providers;

public sealed class UpdateQueryStructureProvider(IRepository<QueryStructureEntity> entityRepository) : IUpdateQueryStructureGateway
{
    private readonly IRepository<QueryStructureEntity> _entityRepository = ArgumentValidator.CheckNull(entityRepository, nameof(entityRepository));

    public async Task<QueryStructure> ExecuteAsync(QueryStructure queryStructure)
    {
        QueryStructureEntity entity = queryStructure.ToEntity();
        
        await UpdateEntity(entity);

        return (await _entityRepository.SaveAsync(entity)).ToModel();
    }

    private async Task UpdateEntity(QueryStructureEntity entity)
    {
        FilterDefinition<QueryStructureEntity> filter = Builders<QueryStructureEntity>.Filter.Eq
        (
            queryStructureEntity => queryStructureEntity.Id,
            entity.Id
        );

        List<UpdateDefinition<QueryStructureEntity>> updateDefinitions =
        [
            Builders<QueryStructureEntity>.Update.Set(queryStructureEntity => queryStructureEntity.UpdatedAt, entity.UpdatedAt)
        ];

        AddUpDefinitionIfValueHasChanged(ref updateDefinitions, nameof(entity.Name), entity.Name, entity.Name);
        AddUpDefinitionIfValueHasChanged(ref updateDefinitions, nameof(entity.Description), entity.Description, entity.Description);
        AddUpDefinitionIfValueHasChanged(ref updateDefinitions, nameof(entity.AiSettings), entity.AiSettings, entity.AiSettings);
        AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, nameof(entity.Items), entity.Items?.ToList(), entity.Items?.ToList());

        UpdateDefinition<QueryStructureEntity> update = Builders<QueryStructureEntity>.Update.Combine(updateDefinitions);
            
        await _entityRepository.UpdateOneAsync(filter, update);
    }
}