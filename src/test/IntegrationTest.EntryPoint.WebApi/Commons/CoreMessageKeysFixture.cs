using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Core.Commons.Extensions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class CoreMessageKeysFixture : IDisposable
{
    private static readonly string CoreProjectFolder = GetProjectsByCsprojFile.Projects["Core"];
    private static readonly string CoreDomainFolder = $"{CoreProjectFolder}/Domains";
    
    private List<string> _messageKeys;

    private bool _disposed;

    ~CoreMessageKeysFixture() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    public IEnumerable<string> GetCoreProjectMessageKeys()
    {
        return _messageKeys ??= LoadCoreProjectMessageKeys();
    }

    private static List<string> LoadCoreProjectMessageKeys()
    {
        string[] pathToFiles = Directory.GetFiles(CoreDomainFolder, "*.cs", SearchOption.AllDirectories);

        List<string> messageKeys = (from pathToFile in pathToFiles
            from line in File.ReadLines(pathToFile)
            where line.Contains("Exception(\"") || line.Contains("WithMessage(\"")
            select line.Contains("Exception(\"")
                ? line.SubstringAfter("Exception(\"").SubstringBefore("\"")
                : line.SubstringAfter("WithMessage(\"").SubstringBefore("\"")).ToList();

        messageKeys.Sort();

        return messageKeys.Distinct().ToList();
    }
}