using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest.Infra.MongoDB.Commons;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class Context : IDisposable
{
    public ServiceResolver ServiceResolver { get; private set; }

    private bool _disposed;

    ~Context() => Dispose(false);

    public Context()
    {
        IConfiguration configuration = InitConfiguration();

        DockerKeeper dockerKeeper = new DockerKeeper(configuration);
        dockerKeeper.DockerComposeUp();

        DatabaseKeeper databaseKeeper = new DatabaseKeeper(configuration);
        databaseKeeper.ClearCollectionsData();

        ServiceResolver = new ServiceResolver(configuration);
    }

    private static IConfiguration InitConfiguration()
    {
        return
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test.settings.json")
                .Build();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }
}