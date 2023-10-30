using EntryPoint.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class UrlHelperTest
{
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to get base url")]
    [InlineData("http", 7029, "http://localhost:7029/api/bar-management/v1/fools")]
    [InlineData("https", 7029, "https://localhost:7029/api/bar-management/v1/fools")]
    [InlineData("http", 80, "http://localhost/api/bar-management/v1/fools")]
    [InlineData("http", null, "http://localhost/api/bar-management/v1/fools")]
    [InlineData("https", 443, "https://localhost/api/bar-management/v1/fools")]
    public void SuccessfulToGetBaseUrl(string scheme, int? port, string expected)
    {
        // Arrange
        HostString hostString = port is null ? new HostString("localhost") : new HostString("localhost", port.Value);

        Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        httpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext.Request.Scheme).Returns(scheme);
        httpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext.Request.Host).Returns(hostString);
        httpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext.Request.PathBase).Returns("/api/bar-management");
        httpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext.Request.Path).Returns("/v1/fools");

        // Act
        string result = UrlHelper.GetBaseUrl(httpContextAccessor.Object.HttpContext);

        // Assert
        Assert.Equal(expected, result);
    }
}