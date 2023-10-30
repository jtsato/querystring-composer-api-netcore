using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Commons.Extensions;
using Core.Commons.Models;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Commons.Exceptions;

public interface IExceptionHandler
{
    Task<IActionResult> HandleAsync(Exception exception);

    public static string MessageFormatter(string messageKey, params object[] args)
    {
        string message = Messages.ResourceManager.GetString(messageKey, CultureInfo.DefaultThreadCurrentCulture) ?? messageKey;
        return args is null ? message : string.Format(message, args);
    }

    public static ResponseStatus BuildHttpResponseStatus(HttpStatusCode httpStatusCode, string message, ValidationException validationException = null)
    {
        ResponseStatus responseStatus = new ResponseStatus((int) httpStatusCode, message);

        if (validationException is null || !validationException.Errors.Any()) return responseStatus;

        validationException.Errors
            .Select(failure => new Field(ToLowerCamelCase(GetPropertyName(failure.PropertyName)), MessageFormatter(failure.ErrorMessage),
                failure.AttemptedValue as string))
            .ToList()
            .ForEach(responseStatus.Fields.Add);

        return responseStatus;
    }

    public static ResponseStatus BuildHttpResponseStatus(HttpStatusCode httpStatusCode, string message, InvalidArgumentException invalidArgumentException)
    {
        ResponseStatus responseStatus = new ResponseStatus((int) httpStatusCode, message);

        invalidArgumentException.FieldErrors
            .Select(CreateField)
            .ToList()
            .ForEach(responseStatus.Fields.Add);

        return responseStatus;
    }

    private static Field CreateField(FieldError fieldError)
    {
        string name = ToLowerCamelCase(GetPropertyName(fieldError.PropertyName));
        string message = MessageFormatter(fieldError.ErrorMessage, fieldError.PropertyName);
        string value = fieldError.AttemptedValue;
        return new Field(name, message, value);
    }

    private static string GetPropertyName(string propertyPath)
    {
        return !propertyPath.Contains('.') ? propertyPath : propertyPath.SubstringAfter(".");
    }

    private static string ToLowerCamelCase(string propertyName)
    {
        return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
    }

    public static async Task<IActionResult> Dispatch(ResponseStatus responseStatus)
    {
        return await ResponseBuilder.BuildResponse((HttpStatusCode) responseStatus.Code, responseStatus);
    }
}