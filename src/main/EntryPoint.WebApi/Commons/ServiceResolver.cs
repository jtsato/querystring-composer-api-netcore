using System;
using System.Collections.Generic;
using Core.Commons;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoint.WebApi.Commons;

public sealed class ServiceResolver : IServiceResolver
{
    private IServiceProvider _serviceProvider;
    private Dictionary<Type, ServiceLifetime> _lifetimeByType;

    public void Setup(IServiceProvider serviceProvider, Dictionary<Type, ServiceLifetime> lifetimeByType = null)
    {
        _serviceProvider = serviceProvider;
        _lifetimeByType = lifetimeByType ?? new Dictionary<Type, ServiceLifetime>(0);
    }

    public T Resolve<T>()
    {
        Type type = typeof(T);

        if (_lifetimeByType.TryGetValue(type, out ServiceLifetime _))
        {
            T service = (T) _serviceProvider.GetService(type);
            return ArgumentValidator.CheckNull(service, nameof(type));
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;

        return serviceProvider.GetService<T>();
    }
}