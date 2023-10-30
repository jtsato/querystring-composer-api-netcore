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
    private const string CurrentProjectName = "IntegrationTest.EntryPoint.WebApi";
    private readonly string _projectRootFolder;

    private List<string> _messageKeys;

    private bool _disposed;

    ~CoreMessageKeysFixture() => Dispose(false);

    public CoreMessageKeysFixture()
    {
        _projectRootFolder = Directory.GetCurrentDirectory().SubstringBefore(CurrentProjectName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<string> GetCoreProjectMessageKeys()
    {
        return _messageKeys ??= LoadCoreProjectMessageKeys(_projectRootFolder);
    }

    private static List<string> LoadCoreProjectMessageKeys(string projectRootFolder)
    {
        string[] pathToFiles =
            Directory.GetFiles($"{projectRootFolder}Core/Domains", "*.cs", SearchOption.AllDirectories);

        List<string> messageKeys = (from pathToFile in pathToFiles
            from line in File.ReadLines(pathToFile)
            where line.Contains("Exception(\"") || line.Contains("WithMessage(\"")
            select line.Contains("Exception(\"")
                ? line.SubstringAfter("Exception(\"").SubstringBefore("\"")
                : line.SubstringAfter("WithMessage(\"").SubstringBefore("\"")).ToList();

        messageKeys.Sort();

        return messageKeys.Distinct().ToList();
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }
}