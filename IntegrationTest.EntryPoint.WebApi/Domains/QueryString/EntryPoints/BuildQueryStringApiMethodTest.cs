using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Domains.QueryStrings.Commands;
using Core.Domains.QueryStrings.Interfaces;
using Core.Domains.QueryStrings.Models;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.QueryStrings.EntryPoints;
using EntryPoint.WebApi.Domains.QueryStrings.Models;
using IntegrationTest.EntryPoint.WebApi.Commons;
using IntegrationTest.EntryPoint.WebApi.Commons.Assertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Domains.QueryString.EntryPoints;

[ExcludeFromCodeCoverage]
[Collection("WebApi Collection Context")]
public sealed class BuildQueryStringApiMethodTest : IDisposable
{
    private readonly ApiMethodInvoker _invoker;
    private readonly BuildQueryStringApiMethod _apiMethod;
    private readonly Mock<IWebRequest> _webRequest;
    private readonly Mock<IBuildQueryStringUseCase> _useCase;

    public BuildQueryStringApiMethodTest(ApiMethodInvokerHolder invokerHolder)
    {
        _invoker = invokerHolder.GetApiMethodInvoker();
        _webRequest = new Mock<IWebRequest>(MockBehavior.Strict);
        _useCase = new Mock<IBuildQueryStringUseCase>(MockBehavior.Strict);
        _apiMethod = new BuildQueryStringApiMethod(
            new BuildQueryStringController(_webRequest.Object, _useCase.Object)
        );
    }

    private bool _disposed;

    ~BuildQueryStringApiMethodTest() => Dispose(false);

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        _webRequest.VerifyAll();
        _useCase.VerifyAll();
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [UseCulture("en-US")]
    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "POST /v1/compositions/{queryName} return http status 500 [Internal Server Error]")]
    public async Task FailToBuildQueryStringIfUnexpectedErrorOccurs()
    {
        // Arrange
        _webRequest
            .Setup(webRequest => webRequest.ClientUid)
            .Returns("d8e6b367-0a9c-4634-9263-09d85bff0d28");

        _useCase
            .Setup(useCase => useCase.ExecuteAsync(new BuildQueryStringCommand("d8e6b367-0a9c-4634-9263-09d85bff0d28", "queryName", "searchTerms")))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync
        (
            () => _apiMethod.BuildQueryString(new BuildQueryStringRequest
                {
                    QueryName = "queryName",
                    Body = new BuildQueryStringRequestBody {SearchTerms = "searchTerms"}
                }
            )
        );

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.InternalServerError, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        const string errorMessage = "An unexpected error has occurred, please try again later!";

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo(500))
            .AndExpectThat(JsonFrom.Path("$.message"), Is<string>.EqualTo(errorMessage))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.Empty());
    }

    [UseCulture("en-US")]
    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "POST /v1/compositions/{queryName} return http status 400 [Bad Request]")]
    public async Task FailToBuildQueryStringIfSearchTermsIsNotProvided()
    {
        // Arrange
        _webRequest
            .Setup(webRequest => webRequest.ClientUid)
            .Returns("d8e6b367-0a9c-4634-9263-09d85bff0d28");

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync
        (
            () => _apiMethod.BuildQueryString(new BuildQueryStringRequest
                {
                    QueryName = "queryName",
                    Body = new BuildQueryStringRequestBody {SearchTerms = null}
                }
            )
        );

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.BadRequest, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        const string errorMessage = "Please correct the errors and send your information again!";

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo(400))
            .AndExpectThat(JsonFrom.Path("$.message"), Is<string>.EqualTo(errorMessage))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.NotEmpty())
            .AndExpectThat(JsonFrom.Path("$.fields[0].name"), Is<string>.EqualTo("searchTerms"))
            .AndExpectThat(JsonFrom.Path("$.fields[0].message"), Is<string>.EqualTo("The 'search term' is required."))
            .AndExpectThat(JsonFrom.Path("$.fields[0].value"), Is<string>.EqualTo(null));
    }

    [UseCulture("en-US")]
    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "POST /v1/compositions/{queryName} return http status 200 [OK]")]
    public async Task SuccessToBuildQueryString()
    {
        // Arrange
        _webRequest
            .Setup(webRequest => webRequest.ClientUid)
            .Returns("d8e6b367-0a9c-4634-9263-09d85bff0d28");

        _useCase
            .Setup(useCase => useCase.ExecuteAsync(new BuildQueryStringCommand("d8e6b367-0a9c-4634-9263-09d85bff0d28", "queryName", "searchTerms")))
            .ReturnsAsync
            (
                new Output
                {
                    ClientUid = "d8e6b367-0a9c-4634-9263-09d85bff0d28",
                    QueryName = "queryName",
                    SearchTerms = "searchTerms",
                    QueryString = "?q=searchTerms",
                    CreatedByAi = false,
                    CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                }
            );

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync
        (
            () => _apiMethod.BuildQueryString(new BuildQueryStringRequest
                {
                    QueryName = "queryName",
                    Body = new BuildQueryStringRequestBody {SearchTerms = "searchTerms"}
                }
            )
        );

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.OK, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.clientUid"), Is<string>.EqualTo("d8e6b367-0a9c-4634-9263-09d85bff0d28"))
            .AndExpectThat(JsonFrom.Path("$.queryName"), Is<string>.EqualTo("queryName"))
            .AndExpectThat(JsonFrom.Path("$.searchTerms"), Is<string>.EqualTo("searchTerms"))
            .AndExpectThat(JsonFrom.Path("$.queryString"), Is<string>.EqualTo("?q=searchTerms"))
            .AndExpectThat(JsonFrom.Path("$.createdByAi"), Is<bool>.EqualTo(false))
            .AndExpectThat(JsonFrom.Path("$.createdAt"), Is<string>.EqualTo("2023-08-04T17:21:30.000"));
    }
}