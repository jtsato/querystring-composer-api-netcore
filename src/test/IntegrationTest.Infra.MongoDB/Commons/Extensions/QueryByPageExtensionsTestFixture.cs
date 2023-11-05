using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Repository;
using IntegrationTest.Infra.MongoDB.Commons.Dummies;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Commons.Extensions;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class QueryByPageExtensionsTestFixture : IDisposable
{
    private readonly IRepository<DummyEntity> _repository;
    private readonly ISequenceRepository<DummySequence> _sequenceRepository;

    public QueryByPageExtensionsTestFixture(Context context)
    {
        _repository = context.ServiceResolver.Resolve<IRepository<DummyEntity>>();
        _sequenceRepository = context.ServiceResolver.Resolve<ISequenceRepository<DummySequence>>();
        
        string[] colors = {"Black", "White", "Red", "Green", "Blue", "Yellow", "Purple", "Orange", "Brown", "Gray"};

        Task[] tasks = new Task[colors.Length];
        
        for (int index = 0; index < colors.Length; index++)
        {
            DummyEntity entity = new DummyEntity
            (
                id: 0,
                name: colors[index],
                surname: "Dummy",
                birthDateAsString: new DateTime(1980, 04, 23, 17, 21, 30, DateTimeKind.Local).ToUniversalTime().ToString("O"),
                age: 43 + index
            );
            
            IncrementId(entity).Wait();

            Task task = _repository.SaveAsync(entity);
            tasks[index] = task;
        }
        
        Task.WaitAll(tasks);
    }
    
    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<DummySequence> filterDefinition = Builders<DummySequence>.Filter.Eq(sequence => sequence.SequenceName, "id");
        ISequence sequence = await _sequenceRepository.GetSequenceAndUpdate(filterDefinition);
        entity.Id = sequence.SequenceValue;
    }
    
    ~QueryByPageExtensionsTestFixture() => Dispose();

    public void Dispose()
    {
        List<FilterDefinition<DummyEntity>> filterDefinitions = new List<FilterDefinition<DummyEntity>>
        {
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Black"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "White"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Red"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Green"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Blue"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Yellow"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Purple"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Orange"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Brown"),
            Builders<DummyEntity>.Filter.Eq(entity => entity.Name, "Gray")
        };

        _repository.DeleteManyAsync(Builders<DummyEntity>.Filter.Or(filterDefinitions));

        GC.SuppressFinalize(this);
    }
}