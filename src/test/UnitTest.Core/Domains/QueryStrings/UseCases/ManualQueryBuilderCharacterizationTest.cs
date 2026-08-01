using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Core.Domains.QueryStrings.UseCases;
using Core.Domains.QueryStructures.Models;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStrings.UseCases;

// Safety net for the query builder rewrite.
//
// The golden theory asserts the outputs the product is supposed to produce. This one asserts nothing
// about correctness: it records what the builder produces today for a wider corpus, so any refactoring
// that is meant to preserve behaviour shows up as an empty diff, and any behaviour change shows up as
// an explicit, reviewable diff on a checked-in file.
//
// To re-record after an intentional behaviour change:
//     UPDATE_SNAPSHOT=1 dotnet test src/test/UnitTest.Core/UnitTest.Core.csproj
[ExcludeFromCodeCoverage]
public sealed class ManualQueryBuilderCharacterizationTest
{
    private const string UpdateSnapshotVariable = "UPDATE_SNAPSHOT";

    private static readonly UTF8Encoding Utf8 = new UTF8Encoding(false);

    private readonly ITestOutputHelper _outputHelper;

    public ManualQueryBuilderCharacterizationTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Manual query builder output matches the recorded snapshot")]
    public void ManualQueryBuilderOutputMatchesTheRecordedSnapshot()
    {
        string actual = Render();
        string snapshotPath = GetSnapshotPath();

        if (ShouldUpdateSnapshot())
        {
            File.WriteAllText(snapshotPath, actual, Utf8);
            _outputHelper.WriteLine($"Snapshot re-recorded at {snapshotPath}");
            return;
        }

        Assert.True(File.Exists(snapshotPath),
            $"Snapshot not found at {snapshotPath}. Record it with {UpdateSnapshotVariable}=1 dotnet test.");

        string expected = Normalize(File.ReadAllText(snapshotPath, Encoding.UTF8));

        Assert.Equal(expected, Normalize(actual));
    }

    private static string Render()
    {
        QueryStructure queryStructure = GeneralPropertiesFixture.Build();

        StringBuilder builder = new StringBuilder()
            .AppendLine("# Manual query builder characterization snapshot.")
            .AppendLine("# Recorded output, not asserted expectations. Do not edit by hand.")
            .AppendLine($"# Re-record with: {UpdateSnapshotVariable}=1 dotnet test src/test/UnitTest.Core/UnitTest.Core.csproj")
            .AppendLine();

        foreach (string input in CharacterizationCorpus.All())
        {
            builder.AppendLine($"IN : {input}");
            builder.AppendLine($"OUT: {BuildOrDescribeFailure(queryStructure, input)}");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    // A throwing input is behaviour too: recording the exception keeps the snapshot green while still
    // failing the moment a refactoring starts or stops throwing.
    private static string BuildOrDescribeFailure(QueryStructure queryStructure, string input)
    {
        try
        {
            return ManualQueryBuilderHelper.Build(queryStructure, input);
        }
        catch (Exception exception)
        {
            return $"<<threw {exception.GetType().Name}>>";
        }
    }

    private static bool ShouldUpdateSnapshot()
    {
        string value = Environment.GetEnvironmentVariable(UpdateSnapshotVariable);

        return !string.IsNullOrWhiteSpace(value) && value != "0";
    }

    private static string GetSnapshotPath([CallerFilePath] string sourceFilePath = "")
    {
        return Path.Combine(Path.GetDirectoryName(sourceFilePath) ?? string.Empty, "ManualQueryBuilder.snapshot.txt");
    }

    private static string Normalize(string value)
    {
        return value.Replace("\r\n", "\n");
    }
}
