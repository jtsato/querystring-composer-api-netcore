using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Commons.Paging;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Repository;

public interface IRepository<T>
{
    IMongoCollection<T> GetCollection();

    Task<T> SaveAsync(T entity);

    Task<Core.Commons.Optional<T>> UpdateOneAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition);

    Task<List<T>> UpdateManyAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition);

    Task<Core.Commons.Optional<T>> FindOneAsync(FilterDefinition<T> filterDefinition);

    Task<Page<T>> FindAllAsync(FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int page, int size);

    Task<List<T>> FindAllAsync(FilterDefinition<T> filterDefinition);

    Task<Core.Commons.Optional<T>> FindOneAndRemoveAsync(FilterDefinition<T> filterDefinition);

    Task<long> DeleteManyAsync(FilterDefinition<T> filterDefinition);
}