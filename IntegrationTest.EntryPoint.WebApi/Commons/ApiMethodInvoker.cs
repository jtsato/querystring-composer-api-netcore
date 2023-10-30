using System;
using System.Threading.Tasks;
using Core.Commons;
using EntryPoint.WebApi.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

public sealed class ApiMethodInvoker
{
    private readonly IExceptionHandler _exceptionHandler;

    public ApiMethodInvoker(IExceptionHandler exceptionHandler)
    {
        _exceptionHandler = ArgumentValidator.CheckNull(exceptionHandler, nameof(exceptionHandler));
    }

    public async Task<ObjectResult> InvokeAsync(Func<Task<IActionResult>> method, string cultureName = null)
    {
        UseCultureAttribute useCultureAttribute = new UseCultureAttribute(cultureName);
        try
        {
            useCultureAttribute.Before(method.Method);
            return (ObjectResult) await method.Invoke();
        }
        catch (Exception exception)
        {
            return (ObjectResult) await _exceptionHandler.HandleAsync(exception);
        }
        finally
        {
            useCultureAttribute.After(method.Method);
        }
    }
}