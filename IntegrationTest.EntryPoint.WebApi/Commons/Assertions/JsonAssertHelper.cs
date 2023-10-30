using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Core.Commons;
using Core.Commons.Extensions;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
public sealed class JsonAssertHelper
{
    private readonly JsonElement _jsonElement;

    private JsonAssertHelper(JsonElement jsonElement)
    {
        _jsonElement = jsonElement;
    }

    public static JsonAssertHelper AssertThat(JsonElement jsonElement)
    {
        return new JsonAssertHelper(jsonElement);
    }

    [ExcludeFromCodeCoverage]
    public JsonAssertHelper AndExpectThat<T>(JsonFrom jsonFrom, Is<T> matcher)
    {
        return matcher.AssertType switch
        {
            AssertType.Equal => Equal(jsonFrom.JsonPath, matcher.Expected),
            AssertType.StartWith => StartWith(jsonFrom.JsonPath, matcher.Expected),
            AssertType.Empty => Empty(jsonFrom.JsonPath),
            AssertType.NotEmpty => NotEmpty(jsonFrom.JsonPath),
            AssertType.Single => Single(jsonFrom.JsonPath),
            AssertType.Count => Count(jsonFrom.JsonPath, Convert.ToInt32(matcher.Expected)),
            AssertType.Null => Null(jsonFrom.JsonPath),
            _ => throw new InvalidOperationException()
        };
    }

    private JsonAssertHelper Equal<T>(string path, T excepted)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Equal(excepted, _jsonElement.SelectToken(path).ToObject<T>());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, excepted, exception);
        }
    }

    private JsonAssertHelper StartWith<T>(string path, T excepted)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.StartsWith(excepted.ToString(), _jsonElement.SelectToken(path).ToString());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, excepted, exception);
        }
    }

    private JsonAssertHelper Empty(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            if (_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Array)
            {
                Assert.Empty(_jsonElement.SelectToken(path).EnumerateArray());
                return this;
            }

            if (_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Null)
            {
                return this;
            }

            Assert.True(string.IsNullOrWhiteSpace(_jsonElement.SelectToken(path).ToString()));
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, "EMPTY", exception);
        }
    }

    private JsonAssertHelper NotEmpty(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            if (_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Array)
            {
                Assert.NotEmpty(_jsonElement.SelectToken(path).EnumerateArray());
                return this;
            }

            Assert.False(_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Null);
            if (_jsonElement.SelectToken(path).ValueKind != JsonValueKind.String) return this;
            Assert.False(string.IsNullOrWhiteSpace(_jsonElement.SelectToken(path).ToString()));
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, "NotEmpty", exception);
        }
    }

    private JsonAssertHelper Single(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Single(_jsonElement.SelectToken(path).EnumerateArray());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, "Single", exception);
        }
    }

    private JsonAssertHelper Count(string path, int excepted)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Equal(excepted, _jsonElement.SelectToken(path).GetArrayLength());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, excepted, exception);
        }
    }

    private JsonAssertHelper Null(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.True(_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Null);
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(path, "Null", exception);
        }
    }

    private static AssertionException CreateAssertionException<T>(string path, T excepted, Exception exception, string message = null)
    {
        StackTrace stackTrace = new StackTrace(3, true);
        exception.SetStackTrace(stackTrace);
        return new AssertionException(exception.StackTrace, $"Assertion Failed: Path {path} expected to be {excepted}. Cause: {message ?? exception.Message}");
    }
}