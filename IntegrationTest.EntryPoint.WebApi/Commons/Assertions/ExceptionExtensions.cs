using System;
using System.Diagnostics;
using System.Reflection;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

public static class ExceptionExtensions
{
    private static readonly FieldInfo FieldInfo = typeof(Exception).GetField("_stackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);
    private static readonly Type Type = typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.NonPublic);

    private static readonly MethodInfo MethodInfo =
        typeof(StackTrace).GetMethod("ToString", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] {Type}, null);

    public static Exception SetStackTrace(this Exception target, StackTrace stack)
    {
        object stackTraceAsString = MethodInfo.Invoke(stack, new[] {Enum.GetValues(Type).GetValue(0)});
        FieldInfo.SetValue(target, stackTraceAsString);
        return target;
    }
}