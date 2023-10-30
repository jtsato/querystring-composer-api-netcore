namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

public readonly struct Is<T>
{
    public AssertType AssertType { get; }
    public T Expected { get; }

    private Is(AssertType assertType)
    {
        AssertType = assertType;
        Expected = default;
    }

    private Is(AssertType assertType, T expected)
    {
        AssertType = assertType;
        Expected = expected;
    }

    public static Is<T> EqualTo(T excepted)
    {
        return new Is<T>(AssertType.Equal, excepted);
    }

    public static Is<T> StartWith(T excepted)
    {
        return new Is<T>(AssertType.StartWith, excepted);
    }

    public static Is<T> Empty()
    {
        return new Is<T>(AssertType.Empty);
    }

    public static Is<T> NotEmpty()
    {
        return new Is<T>(AssertType.NotEmpty);
    }

    public static Is<T> Single()
    {
        return new Is<T>(AssertType.Single);
    }

    public static Is<int> Count(int excepted)
    {
        return new Is<int>(AssertType.Count, excepted);
    }

    public static Is<T> Null()
    {
        return new Is<T>(AssertType.Null);
    }
}