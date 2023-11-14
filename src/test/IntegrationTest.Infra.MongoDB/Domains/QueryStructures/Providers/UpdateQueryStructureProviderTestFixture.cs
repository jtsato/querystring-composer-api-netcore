using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class UpdateQueryStructureProviderTestFixture : IDisposable
{
    private readonly IRepository<QueryStructureEntity> _repository;
    private readonly ISequenceRepository<QueryStructureSequence> _sequenceRepository;

    public UpdateQueryStructureProviderTestFixture(Context context)
    {
        _repository = context.ServiceResolver.Resolve<IRepository<QueryStructureEntity>>();
        _sequenceRepository = context.ServiceResolver.Resolve<ISequenceRepository<QueryStructureSequence>>();

        QueryStructureEntity entity = new QueryStructureEntity
        {
            ClientUid = "490f1db4-ed14-4cdc-a09f-401048951b18",
            Name = "existing-query-structure",
            Description = "Existing query structure",
            AiSettings = new AiSettingsEntity
            {
                UsagePercentage = 50,
                ApiKey = "api-key",
                Model = "model",
                Temperature = 0.2f,
                MaxTokens = 100,
                PromptTemplate = "prompt-template"
            },
            CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local).ToUniversalTime(),
            UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local).ToUniversalTime(),
            Items = new List<ItemEntity>
            {
                new ItemEntity
                {
                    Rank = 1,
                    Name = "item-1",
                    Description = "Item 1",
                    IsCountable = true,
                    WaitForConfirmationWords = true,
                    Entries = new List<EntryEntity>
                    {
                        new EntryEntity
                        {
                            Rank = 1,
                            Key = "entry-1",
                            Exclusive = true,
                            Immiscible = true,
                            KeyWords = new List<string>
                            {
                                "confirmation-word-1",
                                "confirmation-word-2"
                            },
                            IncompatibleWith = new Dictionary<string, string>
                            {
                                {"incompatible-with-key-1", "incompatible-with-value-1"},
                                {"incompatible-with-key-2", "incompatible-with-value-2"}
                            },
                        }
                    }
                }
            }
        };

        IncrementId(entity).Wait();

        Task task = _repository.SaveAsync(entity);

        task.Wait();
    }

    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<QueryStructureSequence> filterDefinition = Builders<QueryStructureSequence>.Filter.Eq(sequence => sequence.SequenceName, "id");
        ISequence sequence = await _sequenceRepository.GetSequenceAndUpdate(filterDefinition);
        entity.Id = sequence.SequenceValue;
    }

    ~UpdateQueryStructureProviderTestFixture() => Dispose();

    public void Dispose()
    {
        List<FilterDefinition<QueryStructureEntity>> filterDefinitions = new List<FilterDefinition<QueryStructureEntity>>
        {
            Builders<QueryStructureEntity>.Filter.Eq(entity => entity.ClientUid, "490f1db4-ed14-4cdc-a09f-401048951b18"),
        };

        _repository.DeleteManyAsync(Builders<QueryStructureEntity>.Filter.Or(filterDefinitions));

        GC.SuppressFinalize(this);
    }
}