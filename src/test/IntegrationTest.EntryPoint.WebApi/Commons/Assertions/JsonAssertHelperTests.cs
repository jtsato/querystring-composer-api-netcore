using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

[ExcludeFromCodeCoverage]
[Collection("WebApi Collection [NoContext]")]
public sealed class JsonAssertHelperTests
{
    private static readonly JsonDocument SampleDocument = JsonDocument.Parse(
        """
        {
            "name": "John",
            "middleName": "",
            "age": 30,
            "isStudent": false,
            "courses": [],
            "friends": [
                "Jane",
                "Mark"
            ],
            "companies": [
              "Microsoft"
            ],
            "reference": null
        }
        """
    );

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with equal to")]
    public void SuccessfulToAssertThatWithEqualTo()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.EqualTo("John"));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with equal to")]
    public void FailToAssertThatWithEqualTo()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.EqualTo("Doe"));
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with start with")]
    public void SuccessfulToAssertThatWithStartWith()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.StartWith("Jo"));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with start with")]
    public void FailToAssertThatWithStartWith()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.StartWith("De"));
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with empty element null")]
    public void SuccessfulToAssertThatWithEmptyElementNull()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.reference"), Is<string>.Empty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with empty element empty")]
    public void SuccessfulToAssertThatWithEmptyElementEmpty()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.middleName"), Is<string>.Empty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with empty int element")]
    public void FailToAssertThatWithEmptyIntElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.ghost"), Is<int>.Empty());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with empty element")]
    public void FailToAssertThatWithEmptyElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.Empty());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with empty array element")]
    public void SuccessfulToAssertThatWithEmptyArrayElement()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.courses"), Is<List<string>>.Empty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with empty array element")]
    public void FailToAssertThatWithEmptyArrayElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.friends"), Is<List<string>>.Empty());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with not empty int element")]
    public void SuccessfulToAssertThatWithNotEmptyIntElement()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.age"), Is<int>.NotEmpty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with not empty element")]
    public void SuccessfulToAssertThatWithNotEmptyElement()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.name"), Is<string>.NotEmpty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with not empty element")]
    public void FailToAssertThatWithNotEmptyElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.reference"), Is<string>.NotEmpty());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with not empty array element")]
    public void SuccessfulToAssertThatWithNotEmptyArrayElement()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.friends"), Is<List<string>>.NotEmpty());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with not empty array element")]
    public void FailToAssertThatWithNotEmptyArrayElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.courses"), Is<object>.NotEmpty());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with single element in array")]
    public void SuccessfulToAssertThatWithSingleElementArray()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.companies"), Is<List<string>>.Single());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with single element in array")]
    public void FailToAssertThatWithSingleElementArray()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.courses"), Is<List<string>>.Single());
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with array count")]
    public void SuccessfulToAssertThatWithArrayCount()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.friends"), Is<int>.Count(2));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with array count")]
    public void FailToAssertThatWithArrayCount()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.friends"), Is<int>.Count(3));
        });
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to assert that with null element")]
    public void SuccessfulToAssertThatWithNullElement()
    {
        JsonElement element = SampleDocument.RootElement;
        JsonAssertHelper.AssertThat(element)
            .AndExpectThat(JsonFrom.Path("$.reference"), Is<object>.Null());
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to assert that with null element")]
    public void FailToAssertThatWithNullElement()
    {
        JsonElement element = SampleDocument.RootElement;
        Assert.Throws<AssertionException>(() =>
        {
            JsonAssertHelper.AssertThat(element)
                .AndExpectThat(JsonFrom.Path("$.name"), Is<object>.Null());
        });
    }
}