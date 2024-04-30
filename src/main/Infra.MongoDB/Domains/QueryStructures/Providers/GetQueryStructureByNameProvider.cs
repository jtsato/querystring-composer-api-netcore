using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Mappers;
using Infra.MongoDB.Domains.QueryStructures.Models;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.QueryStructures.Providers;

public class GetQueryStructureByNameProvider : IGetQueryStructureByNameGateway
{
    private readonly IRepository<QueryStructureEntity> _clientRepository;
    private readonly ObjectCache _cache = MemoryCache.Default;

    public GetQueryStructureByNameProvider(IRepository<QueryStructureEntity> clientRepository)
    {
        _clientRepository = ArgumentValidator.CheckNull(clientRepository, nameof(clientRepository));
    }

    public async Task<Core.Commons.Optional<QueryStructure>> ExecuteAsync(string clientUid, string name)
    {
        Core.Commons.Optional<QueryStructure> cached = await GetCachedResult(clientUid, name);
        if (cached.HasValue()) return cached;
        
        FilterDefinition<QueryStructureEntity> filterDefinitions = GetFilterDefinition(clientUid, name);
        Core.Commons.Optional<QueryStructureEntity> optionalEntity = await _clientRepository.FindOneAsync(filterDefinitions);
        Core.Commons.Optional<QueryStructure> optional = optionalEntity.Map(QueryStructureMapper.ToModel);

        if (!optional.HasValue()) return optional;
        
        QueryStructure queryStructure = optional.GetValue();
        AddToCache(clientUid, name, queryStructure);

        return optional;
    }
    
    private Task<Core.Commons.Optional<QueryStructure>> GetCachedResult(string clientUid, string name)
    {
        string cacheKey = GetCacheKey(clientUid, name);
        if (!_cache.Contains(cacheKey)) return Task.FromResult(Core.Commons.Optional<QueryStructure>.Empty());

        QueryStructure queryStructure = (QueryStructure) _cache.Get(cacheKey);
        Core.Commons.Optional<QueryStructure> optional = Core.Commons.Optional<QueryStructure>.Of(queryStructure);
        
        return Task.FromResult(optional);
    }
    
    private static string GetCacheKey(string clientUid, string name) => $"QueryStructure_{clientUid}_{name}";
    
    private void AddToCache(string clientUid, string name, QueryStructure queryStructure)
    {
        string cacheKey = GetCacheKey(clientUid, name);
        CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60) };
        _cache.Set(cacheKey, queryStructure, policy);
    }
    
    private static FilterDefinition<QueryStructureEntity> GetFilterDefinition(string clientUid, string name)
    {
        IList<FilterDefinition<QueryStructureEntity>> filters = new List<FilterDefinition<QueryStructureEntity>>();

        FilterHelper.AddEqualsFilter(filters, entity => entity.ClientUid, clientUid);
        FilterHelper.AddEqualsFilter(filters, entity => entity.Name, name);

        return filters.Count == 0 ? Builders<QueryStructureEntity>.Filter.Empty : Builders<QueryStructureEntity>.Filter.And(filters);
    }
}
