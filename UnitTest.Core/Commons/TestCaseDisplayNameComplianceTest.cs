using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Core.Commons.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Commons;

public sealed class TestCaseDisplayNameComplianceTest
{
    private const string CurrentProjectName = "UnitTest.Core";
    private static readonly string[] ExcludedDisplayNamePrefixes = {"POST", "GET", "PUT", "DELETE"};

    private readonly ITestOutputHelper _outputHelper;

    public TestCaseDisplayNameComplianceTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Successful to validate if method name and display name match")]
    [InlineData("[Fact(DisplayName = \"", "\")]")]
    [InlineData("[Theory(DisplayName = \"", "\")]")]
    public void SuccessfulToValidateIfMethodNameAndDisplayNameMatch(string stringPrefixSearch, string stringSuffixSearch)
    {
        // Arrange
        string projectRootFolder = Directory.GetCurrentDirectory().SubstringBefore(CurrentProjectName);
        string[] pathToFiles = Directory.GetFiles($"{projectRootFolder}", "*Test.cs", SearchOption.AllDirectories);

        // Act
        List<NonCompliance> nonCompliances = new List<NonCompliance>();

        foreach (string pathToFile in pathToFiles)
        {
            nonCompliances.AddRange(GetNonCompliancesByFile(projectRootFolder, pathToFile, stringPrefixSearch, stringSuffixSearch));
        }

        nonCompliances.Sort();

        PrintResults(nonCompliances);

        // Assert
        Assert.True(nonCompliances.Count == 0, $"Oh No! {nonCompliances.Count} non compliances were found!");
    }

    [ExcludeFromCodeCoverage]
    private static IEnumerable<NonCompliance> GetNonCompliancesByFile
    (
    	string projectRootFolder,
    	string pathToFile,
    	string stringPrefixSearch,
        string stringSuffixSearch
    )
    {
        string location = pathToFile.SubstringAfter(projectRootFolder);

        List<string> lines = File.ReadLines(pathToFile).ToList();

        string displayName = string.Empty;
        int lineNumber = 0;

        List<NonCompliance> nonCompliances = new List<NonCompliance>();

        foreach (string line in CollectionsMarshal.AsSpan(lines))
        {
            lineNumber++;

            if (line.Contains(stringPrefixSearch) && line.Contains(stringSuffixSearch))
            {
                displayName = line.SubstringAfter(stringPrefixSearch).SubstringBefore(stringSuffixSearch);
                if (Array.Exists(ExcludedDisplayNamePrefixes, prefix => displayName.StartsWith(prefix)))
                {
                    displayName = string.Empty;
                }

                continue;
            }

            if (string.IsNullOrEmpty(displayName)) continue;

            if (line.Contains("[InlineData") || !line.Contains('(') || !line.Contains(')')) continue;

            string methodName = line.SubstringBefore("(").SubstringAfterLast(" ");
            string methodNameAsDisplayName = GetMethodNameAsDisplayName(displayName);

            if (methodName.Equals(methodNameAsDisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                displayName = string.Empty;
                continue;
            }

            NonCompliance nonCompliance = new NonCompliance
            {
                Location = location,
                LineNumber = lineNumber,
                MethodName = methodName,
                MethodNameAsDisplayName = methodNameAsDisplayName,
                ActualDisplayName = displayName
            };

            nonCompliances.Add(nonCompliance);

            displayName = string.Empty;
        }

        return nonCompliances;
    }

    [ExcludeFromCodeCoverage]
    private void PrintResults(List<NonCompliance> nonCompliances)
    {
        foreach (NonCompliance nonCompliance in CollectionsMarshal.AsSpan(nonCompliances))
        {
            _outputHelper.WriteLine("Location: {0}:line {1} {2}", nonCompliance.Location, nonCompliance.LineNumber, nonCompliance.MethodName);
            _outputHelper.WriteLine("MethodName (Actual)  : {0}", nonCompliance.MethodName);
            _outputHelper.WriteLine("MethodName (Expected): {0} -> {1}", nonCompliance.MethodNameAsDisplayName, nonCompliance.ActualDisplayName);
            _outputHelper.WriteLine("");
        }
    }

    [ExcludeFromCodeCoverage]
    private static string GetMethodNameAsDisplayName(string input)
    {
        if (input.Length <= 1) return string.Empty;

        string[] words = RemoveSpecialCharacters(input).Split(" ");
        StringBuilder stringBuilder = new StringBuilder();

        foreach (string element in words)
        {
            if (string.IsNullOrEmpty(element)) continue;
            if (input.Length == 1)
            {
                stringBuilder.Append(char.ToUpperInvariant(element[0]));
                continue;
            }

            stringBuilder.Append(char.ToUpperInvariant(element[0]) + element[1..]);
        }

        return stringBuilder.ToString();
    }

    private static string RemoveSpecialCharacters(string input)
    {
        StringBuilder stringBuilder = new StringBuilder();

        IEnumerable<char> elements = input.Where(element => element is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or ' ');

        foreach (char element in elements)
        {
            stringBuilder.Append(element);
        }

        return stringBuilder.ToString();
    }

    [ExcludeFromCodeCoverage]
    private sealed class NonCompliance : IComparable
    {
        public string Location { get; init; }
        public int LineNumber { get; init; }
        public string MethodName { get; init; }
        public string MethodNameAsDisplayName { get; init; }
        public string ActualDisplayName { get; init; }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NonCompliance) obj);
        }

        private bool Equals(NonCompliance other)
        {
            return Location == other.Location
                   && LineNumber == other.LineNumber
                   && MethodName == other.MethodName
                   && MethodNameAsDisplayName == other.MethodNameAsDisplayName
                   && ActualDisplayName == other.ActualDisplayName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, LineNumber, MethodName, MethodNameAsDisplayName, ActualDisplayName);
        }

        public override string ToString()
        {
            return $"{nameof(Location)}: {Location}, {nameof(LineNumber)}: {LineNumber}, {nameof(MethodName)}: {MethodName}";
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            return obj.GetType() != GetType() ? 1 : CompareTo((NonCompliance) obj);
        }

        private int CompareTo(NonCompliance nonCompliance)
        {
            int value = string.Compare(Location, nonCompliance.Location, StringComparison.Ordinal);
            return value != 0 ? value : nonCompliance.LineNumber.CompareTo(LineNumber);
        }
    }
}