using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons;

public readonly struct Optional<T>
{
    private readonly T _value;

    private Optional(T value)
    {
        _value = value;
    }

    public static Optional<T> Empty() => new Optional<T>(default);

    public static Optional<T> Of([AllowNull] T value)
    {
        return value is null ? Empty() : new Optional<T>(value);
    }

    public T GetValue()
    {
        if (HasValue()) return _value;
        throw new InvalidOperationException("No value present");
    }

    public bool HasValue()
    {
        return _value is not null;
    }

    public void HasValue(Action<T> method)
    {
        if (HasValue()) method.Invoke(_value);
    }

    public void HasValue(Func<object> method)
    {
        if (HasValue()) method.Invoke();
    }

    public T OrElse(T other)
    {
        return HasValue() ? _value : other;
    }

    public T OrElseGet(Func<T> method)
    {
        return HasValue() ? _value : method.Invoke();
    }

    public T OrElseThrow(Func<Exception> method)
    {
        return HasValue() ? _value : throw method.Invoke();
    }

    public Optional<TU> Map<TU>(Func<T, TU> method)
    {
        return HasValue() ? new Optional<TU>(method.Invoke(_value)) : default;
    }
}