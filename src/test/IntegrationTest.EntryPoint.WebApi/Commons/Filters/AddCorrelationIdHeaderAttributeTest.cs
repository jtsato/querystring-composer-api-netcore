using System.Collections.Generic;
using EntryPoint.WebApi.Commons.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Filters;

[Collection("WebApi Collection [NoContext]")]
public sealed class AddCorrelationIdHeaderAttributeTest
{
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to add correlation id to response headers")]
    public void SuccessfulToAddCorrelationIdToResponseHeaders()
    {
        // Arrange
        ActionContext actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext
            {
                Request =
                {
                    Headers = {new KeyValuePair<string, StringValues>("X-Correlation-Id", "d12df24d-1268-42d9-8d0d-d614c8d26299")}
                }
            },
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        ResultExecutingContext resultExecutingContext = new ResultExecutingContext
        (
            actionContext,
            new List<IFilterMetadata>(),
            new ObjectResult("A dummy result from the action method."),
            Mock.Of<Controller>()
        );

        // Act
        AddCorrelationIdHeaderAttribute addCorrelationIdHeaderAttribute = new AddCorrelationIdHeaderAttribute();
        addCorrelationIdHeaderAttribute.OnResultExecuting(resultExecutingContext);

        // Assert
        Assert.Single(resultExecutingContext.HttpContext.Response.Headers);
        Assert.True(resultExecutingContext.HttpContext.Response.Headers.ContainsKey("X-Correlation-Id"));
        Assert.Equal("d12df24d-1268-42d9-8d0d-d614c8d26299", resultExecutingContext.HttpContext.Response.Headers["X-Correlation-Id"]);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to add correlation id to response headers even if request does not contain correlation id")]
    public void SuccessfulToAddCorrelationIdToResponseHeadersEvenIfRequestDoesNotContainCorrelationId()
    {
        // Arrange
        ActionContext actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        ResultExecutingContext resultExecutingContext = new ResultExecutingContext
        (
            actionContext,
            new List<IFilterMetadata>(),
            new ObjectResult("A dummy result from the action method."),
            Mock.Of<Controller>()
        );

        // Act
        AddCorrelationIdHeaderAttribute addCorrelationIdHeaderAttribute = new AddCorrelationIdHeaderAttribute();
        addCorrelationIdHeaderAttribute.OnResultExecuting(resultExecutingContext);

        // Assert
        Assert.Single(resultExecutingContext.HttpContext.Response.Headers);
        Assert.True(resultExecutingContext.HttpContext.Response.Headers.ContainsKey("X-Correlation-Id"));
        Assert.False(string.IsNullOrWhiteSpace(resultExecutingContext.HttpContext.Response.Headers["X-Correlation-Id"]));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to generate correlation id if it is missing from request headers")]
    public void SuccessfulToGenerateCorrelationIdIfItIsMissingFromRequestHeaders()
    {
        // Arrange
        ActionContext actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext
            {
                Request =
                {
                    Headers = {new KeyValuePair<string, StringValues>("X-Correlation-Id", string.Empty)}
                }
            },
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        ResultExecutingContext resultExecutingContext = new ResultExecutingContext
        (
            actionContext,
            new List<IFilterMetadata>(),
            new ObjectResult("A dummy result from the action method."),
            Mock.Of<Controller>()
        );

        // Act
        AddCorrelationIdHeaderAttribute addCorrelationIdHeaderAttribute = new AddCorrelationIdHeaderAttribute();
        addCorrelationIdHeaderAttribute.OnResultExecuting(resultExecutingContext);

        // Assert
        Assert.Single(resultExecutingContext.HttpContext.Response.Headers);
        Assert.True(resultExecutingContext.HttpContext.Response.Headers.ContainsKey("X-Correlation-Id"));

        StringValues stringValues = resultExecutingContext.HttpContext.Response.Headers["X-Correlation-Id"];
        Assert.Single(stringValues);

        Assert.NotNull(stringValues[0]);
        Assert.Equal(36, stringValues[0].Length);
    }
}