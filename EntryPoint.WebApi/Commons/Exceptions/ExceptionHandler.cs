using System;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Commons.Exceptions;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async Task<IActionResult> HandleAsync(Exception exception)
    {
        return exception switch
        {
            InvalidArgumentException invalidArgumentException => await IExceptionHandler.Dispatch(HandleInvalidArgumentException(invalidArgumentException)),
            ValidationException validationException => await IExceptionHandler.Dispatch(HandleValidationException(validationException)),
            NotFoundException => await IExceptionHandler.Dispatch(HandleNotFoundException(exception)),
            UniqueConstraintException => await IExceptionHandler.Dispatch(HandleUniqueConstraintException(exception)),
            ParentConstraintException => await IExceptionHandler.Dispatch(HandleParentConstraintException(exception)),
            AccessDeniedException => await IExceptionHandler.Dispatch(HandleAccessDeniedException(exception)),
            _ => await IExceptionHandler.Dispatch(HandleException())
        };
    }

    private static ResponseStatus HandleInvalidArgumentException(CoreException exception)
    {
        InvalidArgumentException invalidArgumentException = (InvalidArgumentException) exception;
        string message = IExceptionHandler.MessageFormatter(invalidArgumentException.Message, invalidArgumentException.Parameters);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, message, invalidArgumentException);
    }

    private static ResponseStatus HandleValidationException(ValidationException validationException)
    {
        string message = IExceptionHandler.MessageFormatter(nameof(Messages.CommonValidationAlert), null);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, message, validationException);
    }

    private static ResponseStatus HandleNotFoundException(Exception exception)
    {
        NotFoundException notFoundException = (NotFoundException) exception;
        string message = IExceptionHandler.MessageFormatter(notFoundException.Message, notFoundException.Parameters);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.NotFound, message);
    }

    private static ResponseStatus HandleUniqueConstraintException(Exception exception)
    {
        UniqueConstraintException uniqueConstraintException = (UniqueConstraintException) exception;
        string message = IExceptionHandler.MessageFormatter(uniqueConstraintException.Message, uniqueConstraintException.Parameters);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, message);
    }

    private static ResponseStatus HandleParentConstraintException(Exception exception)
    {
        ParentConstraintException parentConstraintException = (ParentConstraintException) exception;
        string message = IExceptionHandler.MessageFormatter(parentConstraintException.Message, parentConstraintException.Parameters);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.BadRequest, message);
    }

    private static ResponseStatus HandleAccessDeniedException(Exception exception)
    {
        AccessDeniedException accessDeniedException = (AccessDeniedException) exception;
        string message = IExceptionHandler.MessageFormatter(accessDeniedException.Message, accessDeniedException.Parameters);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.Unauthorized, message);
    }

    private static ResponseStatus HandleException()
    {
        string message = IExceptionHandler.MessageFormatter("CommonUnexpectedException", null);
        return IExceptionHandler.BuildHttpResponseStatus(HttpStatusCode.InternalServerError, message);
    }
}