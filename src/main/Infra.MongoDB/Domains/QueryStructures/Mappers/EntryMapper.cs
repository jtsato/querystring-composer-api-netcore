using System.Collections.Generic;
using Core.Commons.Extensions;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Models;

namespace Infra.MongoDB.Domains.QueryStructures.Mappers;

public static class EntryMapper
{
    public static IList<Entry> ToModel(this IList<EntryEntity> entities)
    {
        IList<Entry> models = new List<Entry>(entities.Count);
        foreach (EntryEntity entity in entities)
        {
            models.Add(entity.ToModel());
        }

        return models;
    }

    private static Entry ToModel(this EntryEntity entity)
    {
        return new Entry
        {
            Rank = entity.Rank,
            Key = entity.Key,
            Exclusive = entity.Exclusive,
            Immiscible = entity.Immiscible,
            KeyWords = entity.KeyWords.AsEmptyIfNull(),
            IncompatibleWith = entity.IncompatibleWith.AsEmptyIfNull()
        };
    }
    
    public static IList<EntryEntity> ToEntity(this IList<Entry> models)
    {
        IList<EntryEntity> entities = new List<EntryEntity>(models.Count);
        foreach (Entry model in models)
        {
            entities.Add(model.ToEntity());
        }

        return entities;
    }

    private static EntryEntity ToEntity(this Entry model)
    {
        return new EntryEntity
        {
            Rank = model.Rank,
            Key = model.Key,
            Exclusive = model.Exclusive,
            Immiscible = model.Immiscible,
            KeyWords = model.KeyWords.AsEmptyIfNull(),
            IncompatibleWith = model.IncompatibleWith.AsEmptyIfNull()
        };
    }
}