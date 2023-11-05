using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons.Paging;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Extensions;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Repository;

[ExcludeFromCodeCoverage]
public abstract class Repository<T> : IRepository<T>
{
    private readonly IMongoCollection<T> _collection;

    protected Repository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
    {
        IMongoDatabase database = connectionFactory.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(collectionName);
    }

    public IMongoCollection<T> GetCollection()
    {
        return _collection;
    }

    public async Task<Page<T>> FindAllAsync(FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int page, int size)
    {
        (int totalPages, long totalOfElements, IReadOnlyList<T> content) = await _collection.AggregateByPage(filterDefinition, sortDefinition, page, size);
        Pageable pageable = new Pageable(page, size, content.Count, totalOfElements, totalPages);
        return new Page<T>(content, pageable);
    }

    public async Task<List<T>> FindAllAsync(FilterDefinition<T> filterDefinition)
    {
        return (await _collection.FindAsync(filterDefinition)).ToList();
    }

    public async Task<T> SaveAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<Core.Commons.Optional<T>> FindOneAsync(FilterDefinition<T> filterDefinition)
    {
        IAsyncCursor<T> cursor = await _collection.FindAsync(filterDefinition);
        T result = await cursor.FirstOrDefaultAsync();
        return Core.Commons.Optional<T>.Of(result);
    }

    public async Task<Core.Commons.Optional<T>> FindOneAndRemoveAsync(FilterDefinition<T> filterDefinition)
    {
        T result = await _collection.FindOneAndDeleteAsync(filterDefinition);
        return Core.Commons.Optional<T>.Of(result);
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<T> filterDefinition)
    {
        DeleteResult deleteManyAsync = await _collection.DeleteManyAsync(filterDefinition);
        return deleteManyAsync.DeletedCount;
    }

    public async Task<Core.Commons.Optional<T>> UpdateOneAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
    {
        await _collection.UpdateOneAsync(filterDefinition, updateDefinition);
        return await FindOneAsync(filterDefinition);
    }

    public async Task<List<T>> UpdateManyAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
    {
        await _collection.UpdateManyAsync(filterDefinition, updateDefinition);
        return (await _collection.FindAsync(filterDefinition)).ToList();
    }
}