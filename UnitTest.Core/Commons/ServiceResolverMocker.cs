using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Moq;

namespace UnitTest.Core.Commons;

[ExcludeFromCodeCoverage]
public sealed class ServiceResolverMocker
{
    private readonly Mock<IServiceResolver> _serviceResolver;
    public IServiceResolver Object => _serviceResolver.Object;

    public ServiceResolverMocker()
    {
        _serviceResolver = new Mock<IServiceResolver>(MockBehavior.Strict);
    }

    public ServiceResolverMocker AddService<T>(T instance)
    {
        _serviceResolver
            .Setup(self => self.Resolve<T>())
            .Returns(instance);

        return this;
    }
}