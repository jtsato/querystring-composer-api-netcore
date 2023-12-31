﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class RegisterClientProviderTestFixture : IDisposable
{
    private readonly IRepository<ClientEntity> _repository;
    private readonly ISequenceRepository<ClientSequence> _sequenceRepository;

    public RegisterClientProviderTestFixture(Context context)
    {
        _repository = context.ServiceResolver.Resolve<IRepository<ClientEntity>>();
        _sequenceRepository = context.ServiceResolver.Resolve<ISequenceRepository<ClientSequence>>();

        ClientEntity entity = new ClientEntity
        {
            Uid = "490f1db4-ed14-4cdc-a09f-401048951b17",
            Name = "already-exists-client",
            Description = "Already exists client",
            CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local).ToUniversalTime(),
            UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local).ToUniversalTime()
        };
        
        IncrementId(entity).Wait();

        Task task = _repository.SaveAsync(entity);
        
        task.Wait();
    }
    
    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<ClientSequence> filterDefinition = Builders<ClientSequence>.Filter.Eq(sequence => sequence.SequenceName, "id");
        ISequence sequence = await _sequenceRepository.GetSequenceAndUpdate(filterDefinition);
        entity.Id = sequence.SequenceValue;
    }

    ~RegisterClientProviderTestFixture() => Dispose();

    public void Dispose()
    {
        List<FilterDefinition<ClientEntity>> filterDefinitions = new List<FilterDefinition<ClientEntity>>
        {
            new FilterDefinitionBuilder<ClientEntity>().Eq(entity => entity.Uid, "490f1db4-ed14-4cdc-a09f-401048951b16"),
            new FilterDefinitionBuilder<ClientEntity>().Eq(entity => entity.Uid, "490f1db4-ed14-4cdc-a09f-401048951b17"),
        };

        _repository.DeleteManyAsync(Builders<ClientEntity>.Filter.Or(filterDefinitions));

        GC.SuppressFinalize(this);
    }
}