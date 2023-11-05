using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Commons.Models;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Models;
using FluentValidation;
using FluentValidation.Results;
using IntegrationTest.EntryPoint.WebApi.Commons.Assertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class ExceptionHandlerTest
{
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to format message if key not exists in the ResourceFile")]
    public void FailToFormatMessageIfKeyNotExistsInTheResourceFile()
    {
        // Arrange
        // Act 
        // Assert
        Assert.Equal("unknownKey", IExceptionHandler.MessageFormatter("unknownKey"));
        Assert.Equal("unknownKey", IExceptionHandler.MessageFormatter("unknownKey", null));
        Assert.Equal("unknownKey", IExceptionHandler.MessageFormatter("unknownKey", "One", "Two", "Three"));
    }

    [UseCulture("en-US", "en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to format message if key exists in the ResourceFile for default culture")]
    public void SuccessfulToFormatMessageIfKeyExistsInTheResourceFileForDefaultCulture()
    {
        // Arrange
        // Act 
        // Assert
        Assert.Equal("Please correct the errors and send your information again!",
            IExceptionHandler.MessageFormatter("CommonValidationAlert"));

        Assert.Equal("An unexpected error has occurred, please try again later!",
            IExceptionHandler.MessageFormatter("CommonUnexpectedException"));

        Assert.Equal("An unexpected error has occurred, please try again later! Details: Value cannot be null or empty. (Parameter 'DummyParameter').",
            IExceptionHandler.MessageFormatter("CommonUnexpectedExceptionWithParams", "Value cannot be null or empty. (Parameter 'DummyParameter')"));
    }

    [UseCulture("pt-BR", "pt-BR")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to format message if key exists in the ResourceFile for pt-BR culture")]
    public void SuccessfulToFormatMessageIfKeyExistsInTheResourceFileForPtBrCulture()
    {
        // Arrange
        // Act 
        // Assert
        Assert.Equal("Favor corrigir os erros e enviar os dados novamente!",
            IExceptionHandler.MessageFormatter("CommonValidationAlert"));

        Assert.Equal("Ocorreu um erro inesperado, por favor tente novamente mais tarde!",
            IExceptionHandler.MessageFormatter("CommonUnexpectedException"));

        Assert.Equal("Ocorreu um erro inesperado, por favor tente novamente mais tarde! Detalhes: Value cannot be null or empty. (Parameter 'DummyParameter').",
            IExceptionHandler.MessageFormatter("CommonUnexpectedExceptionWithParams", "Value cannot be null or empty. (Parameter 'DummyParameter')"));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to build http response status")]
    public void SuccessfulToBuildHttpResponseStatus()
    {
        // Arrange
        // Act 
        // Assert
        Assert.Equal(new ResponseStatus((int) HttpStatusCode.BadRequest, "Please correct the errors and send your information again!"),
            IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, "Please correct the errors and send your information again!"));

        Assert.Equal(new ResponseStatus((int) HttpStatusCode.BadRequest, "Please correct the errors and send your information again!"),
            IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, "Please correct the errors and send your information again!",
                new ValidationException("")));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to build http response status handling validation exception")]
    public void SuccessfulToBuildHttpResponseStatusHandlingValidationException()
    {
        ValidationException validationException = new ValidationException(new List<ValidationFailure>
        {
            new ValidationFailure("id", "The identifier is required.")
        });

        ResponseStatus expected = new ResponseStatus((int) HttpStatusCode.BadRequest, "Please correct the errors and send your information again!");
        expected.Fields.Add(new Field("id", "The identifier is required.", null));

        Assert.Equal(expected, IExceptionHandler.BuildHttpResponseStatus(
            HttpStatusCode.BadRequest, "Please correct the errors and send your information again!", validationException));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to build http response status handling validation exception with nested object")]
    public void SuccessfulToBuildHttpResponseStatusHandlingValidationExceptionWithNestedObject()
    {
        ValidationException validationException = new ValidationException(new List<ValidationFailure>
        {
            new ValidationFailure("dummy.id", "The identifier is required.")
        });

        ResponseStatus expected = new ResponseStatus((int) HttpStatusCode.BadRequest, "Please correct the errors and send your information again!");
        expected.Fields.Add(new Field("id", "The identifier is required.", null));

        Assert.Equal(expected, IExceptionHandler.BuildHttpResponseStatus(
            HttpStatusCode.BadRequest, "Please correct the errors and send your information again!", validationException));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to build http response status handling invalid argument exception")]
    public void SuccessfulToBuildHttpResponseStatusHandlingInvalidArgumentException()
    {
        InvalidArgumentException invalidArgumentException = new InvalidArgumentException("Please correct the errors and send your information again!",
            new List<FieldError>
            {
                new FieldError
                {
                    PropertyName = "id",
                    ErrorMessage = "The identifier is required.",
                }
            });

        ResponseStatus expected = new ResponseStatus((int) HttpStatusCode.BadRequest, "Please correct the errors and send your information again!");
        expected.Fields.Add(new Field("id", "The identifier is required.", null));

        Assert.Equal(expected, IExceptionHandler.BuildHttpResponseStatus(
            HttpStatusCode.BadRequest, "Please correct the errors and send your information again!", invalidArgumentException));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to handle exception")]
    [InlineData(typeof(ParentConstraintException), HttpStatusCode.BadRequest)]
    [InlineData(typeof(UniqueConstraintException), HttpStatusCode.BadRequest)]
    [InlineData(typeof(InvalidArgumentException), HttpStatusCode.BadRequest)]
    [InlineData(typeof(ValidationException), HttpStatusCode.BadRequest)]
    [InlineData(typeof(AccessDeniedException), HttpStatusCode.Unauthorized)]
    [InlineData(typeof(NotFoundException), HttpStatusCode.NotFound)]
    [InlineData(typeof(ServiceUnavailableException), HttpStatusCode.InternalServerError)]
    [InlineData(typeof(Exception), HttpStatusCode.InternalServerError)]
    public async Task SuccessfulToHandleException(Type type, HttpStatusCode expectedHttpStatusCode)
    {
        // Arrange
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        // Act 
        IActionResult actionResult = await exceptionHandler.HandleAsync(CreateExceptionByType(type));
        ObjectResult objectResult = (ObjectResult) actionResult;

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) expectedHttpStatusCode, objectResult.StatusCode);

        Assert.NotNull(objectResult.Value);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo((int) expectedHttpStatusCode))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.Empty());
    }

    private static Exception CreateExceptionByType(Type type)
    {
        if (type == typeof(ValidationException))
            return CreateInstance<ValidationException>("CommonValidationAlert");

        const string messageKey = "CommonUnexpectedException";

        if (type == typeof(InvalidArgumentException))
            return CreateInstance<InvalidArgumentException>(messageKey, new List<FieldError>(0));

        if (type == typeof(ParentConstraintException))
            return CreateInstance<ParentConstraintException>(messageKey);

        if (type == typeof(UniqueConstraintException))
            return CreateInstance<UniqueConstraintException>(messageKey);

        if (type == typeof(AccessDeniedException))
            return CreateInstance<AccessDeniedException>(messageKey);

        if (type == typeof(NotFoundException))
            return CreateInstance<NotFoundException>(messageKey);

        if (type == typeof(ServiceUnavailableException))
            return CreateInstance<ServiceUnavailableException>(messageKey);

        return CreateInstance<Exception>(messageKey);
    }

    private static T CreateInstance<T>(params object[] paramArray)
    {
        return (T) Activator.CreateInstance(typeof(T), paramArray);
    }
}