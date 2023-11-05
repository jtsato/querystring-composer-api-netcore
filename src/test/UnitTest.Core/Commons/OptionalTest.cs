using System;
using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Xunit;

namespace UnitTest.Core.Commons;

public sealed class OptionalTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to create an optional with no content")]
    public void SuccessfulToCreateAnOptionalWithNoContent()
    {
        // Arrange
        // Act
        // Assert
        Assert.False(Optional<DummyClass>.Empty().HasValue());
        Assert.Equal(Optional<DummyClass>.Empty(), Optional<DummyClass>.Empty());
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get value if optional contains content")]
    public void SuccessfulToGetValueIfOptionalContainsContent()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Assert
        Assert.True(optional.HasValue());
        DummyClass dummy = optional.GetValue();
        Assert.NotNull(dummy);
        Assert.Equal("X", dummy.Foo);
        Assert.Equal("Y", dummy.Bar);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to create an optional of null content")]
    public void SuccessfulToCreateAnOptionalOfNullContent()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        Assert.False(optional.HasValue());
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to get value if optional has no content")]
    public void FailToGetValueIfOptionalHasNoContent()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => optional.GetValue());
        Assert.Equal("No value present", exception.Message);
    }

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get value if optional contains content ignoring alternative")]
    public void SuccessfulToGetValueIfOptionalContainsContentIgnoringAlternative()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Assert
        Assert.True(optional.HasValue());
        Assert.Equal(new DummyClass("X", "Y"), optional.OrElse(new DummyClass("A", "B")));
        Assert.Equal(new DummyClass("X", "Y"), optional.OrElseGet(() => new DummyClass("A", "B")));
    }

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to returns an alternative if optional has no content")]
    public void SuccessfulToReturnsAnAlternativeIfOptionalHasNoContent()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        Assert.False(optional.HasValue());
        Assert.Equal(new DummyClass("X", "Y"), optional.OrElse(new DummyClass("X", "Y")));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to returns an alternative if optional has no content using lambda")]
    public void SuccessfulToReturnsAnAlternativeIfOptionalHasNoContentUsingLambda()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        Assert.False(optional.HasValue());
        Assert.Equal(new DummyClass("X", "Y"), optional.OrElseGet(() => new DummyClass("X", "Y")));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to throws exception if the optional has no content")]
    public void SuccessfulToThrowsExceptionIfTheOptionalHasNoContent()
    {
        // Arrange
        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        Assert.False(optional.HasValue());
        Assert.Throws<Exception>(() =>
            optional.OrElseThrow(DummyException()));
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to perform a method if the optional has content")]
    public void SuccessfulToPerformAMethodIfTheOptionalHasContent()
    {
        // Arrange
        bool flag = false;

        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Assert
        Assert.True(optional.HasValue());
        optional.HasValue(() => flag = true);
        Assert.True(flag);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to perform a method with parameters if the optional has content")]
    public void SuccessfulToPerformAMethodWithParametersIfTheOptionalHasContent()
    {
        // Arrange
        bool flag = false;

        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Assert
        Assert.True(optional.HasValue());
        optional.HasValue(dummyClass => flag = dummyClass is not null);
        Assert.True(flag);
    }

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to perform a method with parameters if the optional has no content")]
    public void FailToPerformAMethodWithParametersIfTheOptionalHasNoContent()
    {
        // Arrange
        bool flag = false;

        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Assert
        Assert.False(optional.HasValue());
        optional.HasValue(dummyClass => flag = dummyClass is null);
        Assert.False(flag);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to perform a method if the optional has no content")]
    public void FailToPerformAMethodIfTheOptionalHasNoContent()
    {
        // Arrange
        DummyStruct dummyStruct = new DummyStruct("X");

        // Act
        Optional<DummyClass> optional = Optional<DummyClass>.Empty();

        // Assert
        Assert.False(optional.HasValue());
        DummyMethod(optional, dummyStruct);
        Assert.Equal("X", dummyStruct.Value);
    }

    [ExcludeFromCodeCoverage]
    private static void DummyMethod(Optional<DummyClass> optional, DummyStruct dummyStruct)
    {
        optional.HasValue(() => DummyMethod(dummyStruct));
    }

    [ExcludeFromCodeCoverage]
    private static object DummyMethod(DummyStruct dummyStruct)
    {
        dummyStruct.Value = "Z";
        return dummyStruct;
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to get Value without throw exception if optional  contains content")]
    public void SuccessfulToGetValueWithoutThrowExceptionIfOptionalContainsContent()
    {
        // Arrange
        // Act            
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Assert            
        Assert.True(optional.HasValue());
        DummyClass dummy = optional.OrElseThrow(DummyException());
        Assert.NotNull(dummy);
        Assert.Equal("X", dummy.Foo);
        Assert.Equal("Y", dummy.Bar);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to map an optional to another optional of different inner types")]
    public void SuccessToMapAnOptionalToAnotherOptionalOfDifferentInnerTypes()
    {
        // Arrange
        Optional<DummyClass> optional = Optional<DummyClass>.Of(new DummyClass("X", "Y"));

        // Act            
        Optional<DummyClassTwo> optionalTwo = optional.Map(DummyMapper.Of);

        // Assert            
        Assert.True(optionalTwo.HasValue());
        DummyClassTwo dummy = optionalTwo.GetValue();
        Assert.NotNull(dummy);
        Assert.Equal("X", dummy.Foo);
        Assert.Equal("Y", dummy.Bar);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to map an empty optional to another empty optional of different inner types")]
    public void SuccessToMapAnEmptyOptionalToAnotherEmptyOptionalOfDifferentInnerTypes()
    {
        // Arrange
        Optional<DummyClass> optional = Optional<DummyClass>.Of(null);

        // Act            
        Optional<DummyClassTwo> optionalTwo = optional.Map(DummyMapper.Of);

        // Assert            
        Assert.False(optionalTwo.HasValue());
    }

    private static Func<Exception> DummyException()
    {
        return () => new Exception("There is no time for us!");
    }

    [ExcludeFromCodeCoverage]
    private sealed class DummyClass
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

        public override int GetHashCode()
        {
            return HashCode.Combine(Foo, Bar);
        }

        public override string ToString()
        {
            return $"{nameof(Foo)}: {Foo}, {nameof(Bar)}: {Bar}";
        }
    }

    [ExcludeFromCodeCoverage]
    private sealed class DummyClassTwo
    {
        public string Foo { get; init; }
        public string Bar { get; init; }

        public DummyClassTwo(string foo, string bar)
        {
            Foo = foo;
            Bar = bar;
        }

        private bool Equals(DummyClassTwo other)
        {
            return Foo == other.Foo && Bar == other.Bar;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is DummyClassTwo other && Equals(other);
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

    private static class DummyMapper
    {
        public static DummyClassTwo Of(DummyClass dummyClass)
        {
            return new DummyClassTwo(dummyClass.Foo, dummyClass.Bar);
        }
    }

    private struct DummyStruct
    {
        public string Value { get; set; }

        public DummyStruct(string value)
        {
            Value = value;
        }
    }
}