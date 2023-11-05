using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Core.Domains.Clients.Gateways;
using Core.Domains.QueryStructures.Gateways;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Providers;
using Infra.MongoDB.Domains.QueryStructures.Repository;
using IntegrationTest.Infra.MongoDB.Commons.Dummies;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest.Infra.MongoDB.Commons;

public sealed class ServiceResolver : IServiceResolver
{
    private IConnectionFactory _connectionFactory;

    private string _databaseName;
    
    private string _dummyCollectionName;
    private string _dummySequenceCollectionName;
    private IRepository<DummyEntity> _dummyRepository;
    private ISequenceRepository<DummySequence> _dummySequenceRepository;

    private string _clientCollectionName;
    private string _clientSequenceCollectionName;
    private IRepository<ClientEntity> _clientRepository;
    private ISequenceRepository<ClientSequence> _clientSequenceRepository;
    
    private string _queryStructureCollectionName;
    private string _queryStructureSequenceCollectionName;
    private IRepository<QueryStructureEntity> _queryStructureRepository;
    private ISequenceRepository<QueryStructureSequence> _queryStructureSequenceRepository;

    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public ServiceResolver(IConfiguration configuration)
    {
        LoadEnvironmentVariables(configuration);
        AddServices();
    }

    [ExcludeFromCodeCoverage]
    public T Resolve<T>()
    {
        Type type = typeof(T);

        if (_services.TryGetValue(type, out object value)) return (T) value;

        string message = $"Could not find the type {type} in {nameof(ServiceResolver)}";
        throw new ArgumentNullException(message, (Exception) null);
    }

    private void LoadEnvironmentVariables(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory(configuration["MONGODB_URL"]);
        _databaseName = configuration["MONGODB_DATABASE"];
        
        _dummyCollectionName = configuration["DUMMY_COLLECTION_NAME"];
        _dummySequenceCollectionName = configuration["DUMMY_SEQUENCE_COLLECTION_NAME"];
        _clientCollectionName = configuration["CLIENT_COLLECTION_NAME"];
        _clientSequenceCollectionName = configuration["CLIENT_SEQUENCE_COLLECTION_NAME"];
        _queryStructureCollectionName = configuration["QUERY_STRUCTURE_COLLECTION_NAME"];
        _queryStructureSequenceCollectionName = configuration["QUERY_STRUCTURE_SEQUENCE_COLLECTION_NAME"];
    }

    private void AddServices()
    {
        _services.Add(typeof(IRepository<DummyEntity>), GetDummyRepository());
        _services.Add(typeof(ISequenceRepository<DummySequence>), GetDummySequenceRepository());
        
        _services.Add(typeof(IRepository<QueryStructureEntity>), GetQueryStructureRepository());
        _services.Add(typeof(ISequenceRepository<QueryStructureSequence>), GetQueryStructureSequenceRepository());
        _services.Add(typeof(IGetQueryStructureByNameGateway), GetQueryStructureByUidGateway());

        _services.Add(typeof(IRepository<ClientEntity>), GetClientRepository());
        _services.Add(typeof(ISequenceRepository<ClientSequence>), GetClientSequenceRepository());
        _services.Add(typeof(IRegisterClientGateway), GetRegisterClientGateway());
        _services.Add(typeof(IRegisterQueryStructureGateway), GetRegisterQueryStructureGateway());
    }

    private IRepository<DummyEntity> GetDummyRepository()
    {
        return _dummyRepository ??= new DummyRepository(_connectionFactory, _databaseName, _dummyCollectionName);
    }
    
    private ISequenceRepository<DummySequence> GetDummySequenceRepository()
    {
        return _dummySequenceRepository ??=
            new DummySequenceRepository(_connectionFactory, _databaseName, _dummySequenceCollectionName);
    }

    private IRepository<QueryStructureEntity> GetQueryStructureRepository()
    {
        return _queryStructureRepository ??=
            new QueryStructureRepository(_connectionFactory, _databaseName, _queryStructureCollectionName);
    }

    private ISequenceRepository<QueryStructureSequence> GetQueryStructureSequenceRepository()
    {
        return _queryStructureSequenceRepository ??=
            new QueryStructureSequenceRepository(_connectionFactory, _databaseName, _queryStructureSequenceCollectionName);
    }

    private IGetQueryStructureByNameGateway GetQueryStructureByUidGateway()
    {
        return new GetQueryStructureByNameProvider(GetQueryStructureRepository());
    }

    private IRepository<ClientEntity> GetClientRepository()
    {
        return _clientRepository ??= new ClientRepository(_connectionFactory, _databaseName, _clientCollectionName);
    }

    private ISequenceRepository<ClientSequence> GetClientSequenceRepository()
    {
        return _clientSequenceRepository ??=
            new ClientSequenceRepository(_connectionFactory, _databaseName, _clientSequenceCollectionName);
    }

    private IRegisterClientGateway GetRegisterClientGateway()
    {
        return new RegisterClientProvider(GetClientRepository(), GetClientSequenceRepository());
    }
    
    private IRegisterQueryStructureGateway GetRegisterQueryStructureGateway()
    {
        return new RegisterQueryStructureProvider(GetQueryStructureRepository(), GetQueryStructureSequenceRepository());
    }
}