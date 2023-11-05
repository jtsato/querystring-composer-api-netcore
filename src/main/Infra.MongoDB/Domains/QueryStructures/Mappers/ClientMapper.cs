using Core.Domains.Clients.Models;
using Infra.MongoDB.Domains.QueryStructures.Models;

namespace Infra.MongoDB.Domains.QueryStructures.Mappers;

public static class ClientMapper
{
    public static Client ToModel(this ClientEntity entity)
    {
        return new Client
        {
            Id = entity.Id,
            Uid = entity.Uid,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt.ToLocalTime(),
            UpdatedAt = entity.UpdatedAt.ToLocalTime()
        };
    }
    
    public static ClientEntity ToEntity(this Client model)
    {
        return new ClientEntity
        {
            Id = model.Id,
            Uid = model.Uid,
            Name = model.Name,
            Description = model.Description,
            CreatedAt = model.CreatedAt.ToUniversalTime(),
            UpdatedAt = model.UpdatedAt.ToUniversalTime()
        };
    }
}