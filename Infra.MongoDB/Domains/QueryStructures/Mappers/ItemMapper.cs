using System.Collections.Generic;
using Core.Commons.Extensions;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Models;

namespace Infra.MongoDB.Domains.QueryStructures.Mappers;

public static class ItemMapper
{
    public static IList<Item> ToModel(this IList<ItemEntity> entities)
    {
        IList<Item> models = new List<Item>(entities.Count);
        foreach (ItemEntity entity in entities)
        {
            models.Add(entity.ToModel());
        }

        return models;
    }

    private static Item ToModel(this ItemEntity entity)
    {
        return new Item
        {
            Rank = entity.Rank,
            Name = entity.Name,
            Description = entity.Description,
            IsCountable = entity.IsCountable,
            WaitForConfirmationWords = entity.WaitForConfirmationWords,
            Entries = entity.Entries.AsEmptyIfNull().ToModel(),
            ConfirmationWords = entity.ConfirmationWords.AsEmptyIfNull(),
            RevocationWords = entity.RevocationWords.AsEmptyIfNull()
        };
    }
    
    public static IList<ItemEntity> ToEntity(this IList<Item> models)
    {
        IList<ItemEntity> entities = new List<ItemEntity>(models.Count);
        foreach (Item model in models)
        {
            entities.Add(model.ToEntity());
        }

        return entities;
    }

    private static ItemEntity ToEntity(this Item model)
    {
        return new ItemEntity
        {
            Rank = model.Rank,
            Name = model.Name,
            Description = model.Description,
            IsCountable = model.IsCountable,
            WaitForConfirmationWords = model.WaitForConfirmationWords,
            Entries = model.Entries.AsEmptyIfNull().ToEntity(),
            ConfirmationWords = model.ConfirmationWords.AsEmptyIfNull(),
            RevocationWords = model.RevocationWords.AsEmptyIfNull()
        };
    }
}