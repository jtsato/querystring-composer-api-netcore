using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons;

public static class ArgumentValidator
{
    public static T CheckNull<T>([AllowNull] T input, string parameterName, string message = null)
    {
        return input is not null ? input : throw CreateArgumentNullException(parameterName, message);
    }

    public static string CheckEmpty([AllowNull] string input, string parameterName, string message = null)
    {
        return !string.IsNullOrWhiteSpace(input) ? input : throw CreateArgumentException(parameterName, message);
    }

    public static IReadOnlyCollection<T> CheckNullOrEmpty<T>(IReadOnlyCollection<T> input, string parameterName, string message = null)
    {
        if (input is not null && input.Count > 0) return input;

        if (input is null) throw CreateArgumentNullException(parameterName, message);

        throw CreateArgumentException(parameterName, message);
    }

    public static void CheckNegative(int input, string parameterName, string message)
    {
        if (input >= 0) return;
        string exceptionMessage = string.IsNullOrEmpty(message) ? $"Value cannot be negative. (Parameter '{parameterName}')" : message;
        throw CreateArgumentException(parameterName, exceptionMessage);
    }

    public static void CheckNegativeOrZero(int input, string parameterName, string message)
    {
        if (input > 0) return;
        string exceptionMessage = string.IsNullOrEmpty(message) ? $"Value cannot be negative or equals to zero. (Parameter '{parameterName}')" : message;
        throw CreateArgumentException(parameterName, exceptionMessage);
    }

    private static ArgumentNullException CreateArgumentNullException(string parameterName, string message)
    {
        return string.IsNullOrEmpty(message) ? new ArgumentNullException(parameterName) : new ArgumentNullException(message, (Exception) null!);
    }

    private static ArgumentException CreateArgumentException(string parameterName, string message)
    {
        return string.IsNullOrEmpty(message)
            ? new ArgumentException($"Value cannot be null or empty. (Parameter '{parameterName}')")
            : new ArgumentException(message, (Exception) null!);
    }
}