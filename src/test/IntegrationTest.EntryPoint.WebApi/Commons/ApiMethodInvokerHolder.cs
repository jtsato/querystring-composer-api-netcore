using System.Diagnostics.CodeAnalysis;
using EntryPoint.WebApi.Commons.Exceptions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ApiMethodInvokerHolder
{
    private ApiMethodInvoker _invoker;

    public ApiMethodInvoker GetApiMethodInvoker()
    {
        return _invoker ??= new ApiMethodInvoker(new ExceptionHandler());
    }
}