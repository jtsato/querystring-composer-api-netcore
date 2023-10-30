using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EntryPoint.WebApi.Commons;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class ServiceResolverTest
{
    private readonly Mock<IServiceProvider> _serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
    private readonly Mock<IServiceScope> _serviceScope = new Mock<IServiceScope>(MockBehavior.Strict);

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to resolve singleton service")]
    public void SuccessfulToResolveSingletonService()
    {
        // Arrange
        _serviceProvider
            .Setup(self => self.GetService(typeof(DummyClass)))
            .Returns(new DummyClass("Black", "White"));

        Dictionary<Type, ServiceLifetime> lifetimeByType = new Dictionary<Type, ServiceLifetime>
        {
            {typeof(DummyClass), ServiceLifetime.Singleton}
        };

        ServiceResolver serviceResolver = new ServiceResolver();
        serviceResolver.Setup(_serviceProvider.Object, lifetimeByType);

        // Act
        DummyClass dummyClass = serviceResolver.Resolve<DummyClass>();

        // Assert
        Assert.Equal(new DummyClass("Black", "White"), dummyClass);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to resolve non singleton service")]
    public void SuccessfulToResolveNonSingletonService()
    {
        // Arrange
        _serviceScope
            .Setup(self => self.ServiceProvider)
            .Returns(_serviceProvider.Object);

        _serviceScope
            .Setup(self => self.Dispose());

        _serviceScopeFactory
            .Setup(self => self.CreateScope())
            .Returns(_serviceScope.Object);

        _serviceProvider
            .Setup(self => self.GetService(typeof(IServiceScopeFactory)))
            .Returns(_serviceScopeFactory.Object);

        _serviceProvider
            .Setup(self => self.GetService(typeof(DummyClass)))
            .Returns(new DummyClass("Black", "White"));

        ServiceResolver serviceResolver = new ServiceResolver();
        serviceResolver.Setup(_serviceProvider.Object);

        // Act
        DummyClass dummyClass = serviceResolver.Resolve<DummyClass>();

        // Assert
        Assert.Equal(new DummyClass("Black", "White"), dummyClass);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Fail to resolve singleton service")]
    public void FailToResolveSingletonService()
    {
        // Arrange
        _serviceProvider
            .Setup(self => self.GetService(typeof(DummyClass)))
            .Returns(null);

        Dictionary<Type, ServiceLifetime> lifetimeByType = new Dictionary<Type, ServiceLifetime>
        {
            {typeof(DummyClass), ServiceLifetime.Transient}
        };

        ServiceResolver serviceResolver = new ServiceResolver();
        serviceResolver.Setup(_serviceProvider.Object, lifetimeByType);

        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => serviceResolver.Resolve<DummyClass>());
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
}