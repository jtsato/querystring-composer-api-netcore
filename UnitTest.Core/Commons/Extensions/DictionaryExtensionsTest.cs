using Core.Commons.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace UnitTest.Core.Commons.Extensions;

public sealed class DictionaryExtensionsTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to returns an appropriate type of empty dictionary")]
    public void SuccessfulToReturnsAnAppropriateTypeOfEmptyDictionary()
    {
        IReadOnlyDictionary<string, int> result = DictionaryExtensions.Empty<string, int>();

        Assert.Empty(result);
        Assert.IsType<ReadOnlyDictionary<string, int>>(result);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to returns an empty dictionary if the dictionary is null")]
    public void SuccessfulToReturnsAnEmptyDictionaryIfTheDictionaryIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = null;

        // Act
        // ReSharper disable once ExpressionIsAlwaysNull
        // ReSharper's "ExpressionIsAlwaysNull" warning is intentionally triggered here to test the 
        // AsEmptyIfNull extension method's behavior with a null dictionary. The method is designed 
        // to handle null values gracefully by returning a new empty dictionary, ensuring no NullReferenceException.

        IDictionary<string, int> result = dictionary.AsEmptyIfNull();

        // Assert
        Assert.True(result.Count == 0);
    }
}