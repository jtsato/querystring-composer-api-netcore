using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit.Sdk;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UseCultureAttribute(string culture, string uiCulture) : BeforeAfterTestAttribute
{

    private CultureInfo _originalCulture;
    private CultureInfo _originalUiCulture;

    public UseCultureAttribute(string culture) : this(culture, culture) { }

    public override void Before(MethodInfo methodUnderTest)
    {
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUiCulture = Thread.CurrentThread.CurrentUICulture;

        SetThreadCultures(culture, uiCulture);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        SetThreadCultures(_originalCulture.Name, _originalUiCulture.Name);
    }

    private static void SetThreadCultures(string culture, string uiCulture)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture, false);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture, false);
        }
            
        if (string.IsNullOrWhiteSpace(uiCulture)) return;
        CultureInfo.CurrentUICulture = new CultureInfo(uiCulture, false);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(uiCulture, false);
    }
}