using Core.Commons.Extensions;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Models;

namespace Infra.MongoDB.Domains.QueryStructures.Mappers;

public static class QueryStructureMapper
{
    public static QueryStructure ToModel(this QueryStructureEntity entity)
    {
        return new QueryStructure
        {
            ClientUid = entity.ClientUid,
            Name = entity.Name,
            Description = entity.Description,
            AiSettings = entity.AiSettings.ToModel(),
            CreatedAt = entity.CreatedAt.ToLocalTime(),
            UpdatedAt = entity.UpdatedAt.ToLocalTime(),
            Items = entity.Items.AsEmptyIfNull().ToModel()
        };
    }

    public static QueryStructureEntity ToEntity(this QueryStructure model)
    {
        return new QueryStructureEntity
        {
            ClientUid = model.ClientUid,
            Name = model.Name,
            Description = model.Description,
            AiSettings = model.AiSettings.ToEntity(),
            CreatedAt = model.CreatedAt.ToUniversalTime(),
            UpdatedAt = model.UpdatedAt.ToUniversalTime(),
            Items = model.Items.AsEmptyIfNull().ToEntity()
        };
    }
}
