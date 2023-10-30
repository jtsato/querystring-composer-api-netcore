using EntryPoint.WebApi.Commons;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class GetCorrelationIdTest
{
    private const string CorrelationIdKey = "X-Correlation-Id";

    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly GetCorrelationId _getCorrelationId;

    public GetCorrelationIdTest()
    {
        _httpContextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        _getCorrelationId = new GetCorrelationId(_httpContextAccessor.Object);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to get correlationId from header")]
    public void SuccessfulToGetCorrelationIdFromHeader()
    {
        // Arrange
        const string correlationId = "ae4328e9-6fb4-4408-98ee-66634503a0d1";

        IHeaderDictionary headers = new HeaderDictionary {{CorrelationIdKey, correlationId}};
        _httpContextAccessor
            .Setup(httpContextAccessor => httpContextAccessor.HttpContext.Request.Headers)
            .Returns(headers);

        // Act
        string result = _getCorrelationId.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ae4328e9-6fb4-4408-98ee-66634503a0d1", result);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to generate correlation id if it is missing")]
    public void SuccessfulToGenerateCorrelationIdIfItIsMissing()
    {
        // Arrange
        IHeaderDictionary headers = new HeaderDictionary {{CorrelationIdKey, string.Empty}};

        _httpContextAccessor
            .Setup(httpContextAccessor => httpContextAccessor.HttpContext.Request.Headers)
            .Returns(headers);

        // Act
        string correlationId = _getCorrelationId.Execute();

        // Assert
        Assert.NotNull(correlationId);
        Assert.Equal(36, correlationId.Length);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to get correlationId If does not exists in context")]
    public void SuccessfulToGetCorrelationIdIfDoesNotExistsInContext()
    {
        // Arrange
        IHeaderDictionary headers = new HeaderDictionary();
        _httpContextAccessor
            .Setup(httpContextAccessor => httpContextAccessor.HttpContext.Request.Headers)
            .Returns(headers);

        // Act
        string correlationId = _getCorrelationId.Execute();

        // Assert
        Assert.NotEmpty(correlationId);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to get correlationId If httpContext is null")]
    public void SuccessfulToGetCorrelationIdIfHttpContextIsNull()
    {
        // Arrange
        _httpContextAccessor
            .Setup(httpContextAccessor => httpContextAccessor.HttpContext)
            .Returns((HttpContext) null);

        // Act
        string correlationId = _getCorrelationId.Execute();

        // Assert
        Assert.NotEmpty(correlationId);
    }
}