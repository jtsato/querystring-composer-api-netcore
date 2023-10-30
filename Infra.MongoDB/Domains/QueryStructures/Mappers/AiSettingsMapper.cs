using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Models;

namespace Infra.MongoDB.Domains.QueryStructures.Mappers;

public static class AiSettingsMapper
{
    public static AiSettings ToModel(this AiSettingsEntity entity)
    {
        return new AiSettings
        {
            UsagePercentage = entity.UsagePercentage,
            ApiKey = entity.ApiKey,
            Model = entity.Model,
            Temperature = entity.Temperature,
            MaxTokens = entity.MaxTokens,
            PromptTemplate = entity.PromptTemplate
        };
    }

    public static AiSettingsEntity ToEntity(this AiSettings model)
    {
        return new AiSettingsEntity
        {
            UsagePercentage = model.UsagePercentage,
            ApiKey = model.ApiKey,
            Model = model.Model,
            Temperature = model.Temperature,
            MaxTokens = model.MaxTokens,
            PromptTemplate = model.PromptTemplate
        };
    }
}