using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitTest.Core.Commons.Extensions;

[ExcludeFromCodeCoverage]
internal sealed class DummyClass
{
    public string Foo { get; }
    public string Bar { get; }

    public DummyClass(string foo, string bar)
    {
        Foo = foo;
        Bar = bar;
    }

    private bool Equals(DummyClass other)
    {
        return Foo == other.Foo && Bar == other.Bar;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is DummyClass other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Foo, Bar);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(Foo)}: {Foo}, {nameof(Bar)}: {Bar}";
    }
}