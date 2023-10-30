using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Domains.Clients.UseCases;
using Core.Domains.QueryStrings.UseCases;
using Core.Domains.QueryStructures.UseCases;
using Xunit;

namespace UnitTest.Core.Domains;

public class ServiceResolverInjectionTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Fail to create an instance if service resolver is not supplied")]
    [InlineData(typeof(BuildQueryStringUseCase))]
    [InlineData(typeof(RegisterClientUseCase))]
    [InlineData(typeof(RegisterQueryStructureUseCase))]
    public void FailToCreateAnInstanceIfServiceResolverIsNotSupplied(Type type)
    {
        // Arrange
        // Act
        // Assert
        TargetInvocationException targetInvocationException = Assert.Throws<TargetInvocationException>(() =>
            CreateInstance(type)
        );

        Exception innerException = targetInvocationException.InnerException;

        Assert.NotNull(innerException);

        Assert.Equal(typeof(ArgumentNullException), innerException.GetType());
        Assert.Equal("Value cannot be null. (Parameter 'serviceResolver')", innerException.Message);
    }

    [ExcludeFromCodeCoverage]
    private static void CreateInstance(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();
        ConstructorInfo constructor = constructors[0];
        object[] parameters = new object[1];
        constructor.Invoke(parameters);
    }
}