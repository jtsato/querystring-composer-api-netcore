using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Core.Domains.Clients.Gateways;
using Core.Domains.QueryStrings.Interfaces;
using Core.Domains.QueryStrings.UseCases;
using Core.Domains.QueryStructures.Gateways;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using Infra.MongoDB.Domains.QueryStructures.Providers;
using Infra.MongoDB.Domains.QueryStructures.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoint.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjector
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("MONGODB_URL") ?? string.Empty;

    private static readonly string DatabaseName =
        Environment.GetEnvironmentVariable("MONGODB_DATABASE") ?? string.Empty;

    private static readonly string QueryStructureCollectionName =
        Environment.GetEnvironmentVariable("QUERY_STRUCTURE_COLLECTION_NAME") ?? string.Empty;

    private static readonly string QueryStructureSequenceCollectionName =
        Environment.GetEnvironmentVariable("QUERY_STRUCTURE_SEQUENCE_COLLECTION_NAME") ?? string.Empty;

    private static readonly string ClientCollectionName =
        Environment.GetEnvironmentVariable("CLIENT_COLLECTION_NAME") ?? string.Empty;

    private static readonly string ClientSequenceCollectionName =
        Environment.GetEnvironmentVariable("CLIENT_SEQUENCE_COLLECTION_NAME") ?? string.Empty;

    public static Dictionary<Type, ServiceLifetime> ConfigureServices(IServiceCollection services)
    {
        AddSharedServices(services);
        AddEntryPointServices(services);
        AddCoreServices(services);
        AddInfraQueryStructureServices(services, new ConnectionFactory(ConnectionString));

        return BuildLifetimeByType(services);
    }

    private static void AddSharedServices(IServiceCollection services)
    {
        services.AddSingleton<IServiceResolver, ServiceResolver>();
        services.AddSingleton<IGetDateTime, GetDateTime>();
        services.AddTransient<ILoggerAdapter, LoggerAdapter<ExceptionHandlerFilterAttribute>>();
    }

    private static void AddEntryPointServices(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();
        services.AddSingleton<IGetCorrelationId, GetCorrelationId>();
    }

    private static void AddCoreServices(IServiceCollection services)
    {
        services.AddSingleton<IBuildQueryStringUseCase, BuildQueryStringUseCase>();
    }

    private static void AddInfraQueryStructureServices(IServiceCollection services, IConnectionFactory connectionFactory)
    {
        services.AddSingleton<IRegisterClientGateway, RegisterClientProvider>();
        services.AddSingleton<IGetQueryStructureByNameGateway, GetQueryStructureByNameProvider>();
        services.AddSingleton<IRegisterQueryStructureGateway, RegisterQueryStructureProvider>();


        services.AddSingleton<IRepository<ClientEntity>>
        (
            _ => new ClientRepository(connectionFactory, DatabaseName, ClientCollectionName)
        );

        services.AddSingleton<ISequenceRepository<ClientSequence>>
        (
            _ => new ClientSequenceRepository(connectionFactory, DatabaseName, ClientSequenceCollectionName)
        );

        services.AddSingleton<IRepository<QueryStructureEntity>>
        (
            _ => new QueryStructureRepository(connectionFactory, DatabaseName, QueryStructureCollectionName)
        );

        services.AddSingleton<ISequenceRepository<QueryStructureSequence>>
        (
            _ => new QueryStructureSequenceRepository(connectionFactory, DatabaseName, QueryStructureSequenceCollectionName)
        );
    }

    private static Dictionary<Type, ServiceLifetime> BuildLifetimeByType(IServiceCollection services)
    {
        Dictionary<Type, ServiceLifetime> lifetimeByType = new Dictionary<Type, ServiceLifetime>();
        foreach (ServiceDescriptor service in services)
        {
            if (service.Lifetime != ServiceLifetime.Singleton) continue;
            if (lifetimeByType.ContainsKey(service.ServiceType)) continue;
            lifetimeByType.Add(service.ServiceType, service.Lifetime);
        }

        return lifetimeByType;
    }
}