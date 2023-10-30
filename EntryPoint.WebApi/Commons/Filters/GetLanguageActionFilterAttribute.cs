using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EntryPoint.WebApi.Commons.Filters;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class GetLanguageActionFilterAttribute : ActionFilterAttribute
{
    private static readonly string[] SupportedCultures = {"pt-BR", "en-US"};

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        SetupLanguage(context.HttpContext.Request.Headers);
    }

    private static void SetupLanguage(IHeaderDictionary headers)
    {
        if (headers.TryGetValue("Accept-Language", out StringValues values))
        {
            SetupLanguage(values.ToList()[0]);
            return;
        }

        SetupLanguage();
    }

    public static void SetupLanguage(string cultureName = null)
    {
        bool isCustomizable = !string.IsNullOrWhiteSpace(cultureName) && SupportedCultures.Contains(cultureName, StringComparer.InvariantCultureIgnoreCase);
        CultureInfo cultureInfo = isCustomizable ? CultureInfo.CreateSpecificCulture(cultureName) : new CultureInfo("en-US");

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }
}