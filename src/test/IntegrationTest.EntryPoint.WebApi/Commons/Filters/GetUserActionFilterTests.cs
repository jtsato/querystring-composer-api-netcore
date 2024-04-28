using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using EntryPoint.WebApi.Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Filters;

[ExcludeFromCodeCoverage]
[Collection("WebApi Collection [NoContext]")]
public sealed class GetUserActionFilterTests : IDisposable
{
    private bool _disposed;

    ~GetUserActionFilterTests() => Dispose(false);

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to get user from token when token is empty")]
    public void FailToGetUserFromTokenWhenTokenIsEmpty()
    {
        // Arrange
        IWebRequest webRequest = new WebRequest();
        GetUserActionFilter actionFilter = new GetUserActionFilter(webRequest);

        DefaultHttpContext httpContext = new DefaultHttpContext();
        RouteData routeData = new RouteData();
        ActionDescriptor actionDescriptor = new ActionDescriptor();

        ActionExecutingContext executingContext = new ActionExecutingContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );

        ActionExecutedContext executedContext = new ActionExecutedContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new object()
        );

        httpContext.Request.Headers["Authorization"] = string.Empty;

        // Act
        Exception exception = Record.Exception(() =>
        {
            actionFilter.OnActionExecuting(executingContext);
            actionFilter.OnActionExecuted(executedContext);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<AccessDeniedException>(exception);
        Assert.Equal("CommonAccessDeniedException", exception.Message);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to get user from token when token is missing")]
    public void FailToGetUserFromTokenWhenTokenIsMissing()
    {
        // Arrange
        IWebRequest webRequest = new WebRequest();
        GetUserActionFilter actionFilter = new GetUserActionFilter(webRequest);

        DefaultHttpContext httpContext = new DefaultHttpContext();
        RouteData routeData = new RouteData();
        ActionDescriptor actionDescriptor = new ActionDescriptor();

        ActionExecutingContext executingContext = new ActionExecutingContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );

        ActionExecutedContext executedContext = new ActionExecutedContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new object()
        );

        // Act
        Exception exception = Record.Exception(() =>
        {
            actionFilter.OnActionExecuting(executingContext);
            actionFilter.OnActionExecuted(executedContext);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<AccessDeniedException>(exception);
        Assert.Equal("CommonAccessDeniedException", exception.Message);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to get user from token")]
    public void SuccessfulToGetUserFromToken()
    {
        // Arrange
        IWebRequest webRequest = new WebRequest();
        GetUserActionFilter actionFilter = new GetUserActionFilter(webRequest);

        DefaultHttpContext httpContext = new DefaultHttpContext();
        RouteData routeData = new RouteData();
        ActionDescriptor actionDescriptor = new ActionDescriptor();

        ActionExecutingContext executingContext = new ActionExecutingContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );

        ActionExecutedContext executedContext = new ActionExecutedContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            },
            Array.Empty<IFilterMetadata>(),
            new object()
        );

        httpContext.Request.Headers["Authorization"] =
            "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE3MTQzMzM5NjgsImV4cCI6MTc0NTg2OTk2OCwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsInVzZXJuYW1lIjoiSm9obiBTbWl0aCIsImVtYWlsIjoidXNlckBleGFtcGxlLmNvbSIsImNsaWVudFVpZCI6Ijk0MTkzNTdlLTEyM2ItNDk0YS04YmMzLWZkMTczNzNjMjE4YyJ9.Q8ePzx52p0WUcbmTgNE-g8tsv3vtsVqyg-JXQ85wwis";

        // Act
        actionFilter.OnActionExecuting(executingContext);
        actionFilter.OnActionExecuted(executedContext);

        // Assert
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218c", webRequest.ClientUid);
        Assert.Equal("user@example.com", webRequest.Email);
        Assert.Equal("John Smith", webRequest.Username);
    }
}