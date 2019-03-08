using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Msh.Microsoft.Extensions.Tests.DependencyInjectionTest")]

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddTransient<TKey>(this IServiceCollection services, TKey key, Type service, Type implementationType)
        {
            return services.TryAdd(key, ServiceDescriptor.Transient(service, implementationType));
        }

        public static IServiceCollection TryAddTransient<TKey, TService, TImplementation>(this IServiceCollection services, TKey key)
            where TService : class
            where TImplementation : class, TService
        {
            return services.TryAdd(key, ServiceDescriptor.Transient<TService, TImplementation>());
        }

        public static IServiceCollection TryAddScoped<TKey, TService, TImplementation>(this IServiceCollection services, TKey key)
            where TService : class
            where TImplementation : class, TService
        {
            return services.TryAdd(key, ServiceDescriptor.Scoped<TService, TImplementation>());
        }

        public static IServiceCollection TryAddSingleton<TKey>(this IServiceCollection services, TKey key, Type serviceType, object implementationInstance)
        {
            return services.TryAdd(key, ServiceDescriptor.Singleton(serviceType, implementationInstance));
        }

        public static IServiceCollection TryAddSingleton<TKey, TService, TImplementation>(this IServiceCollection services, TKey key)
            where TService : class
            where TImplementation : class, TService
        {
            return services.TryAdd(key, ServiceDescriptor.Singleton<TService, TImplementation>());
        }

        // TODO: all overwrites for Transient/Scoped/Singleton

        public static IServiceCollection TryAdd<TKey>(this IServiceCollection services, TKey key, ServiceDescriptor descriptor)
        {
            Func<TKey, ServiceDescriptor, ServiceDescriptor> tryAddService = DelegateHelper.GetTryAddServiceMethodOfRegisteredServicesSingleton<TKey>(descriptor.ServiceType); 
            TryAdd(tryAddService, services, key, descriptor);
            return services;
        }

        internal static void TryAdd<TKey>(Func<TKey, ServiceDescriptor, ServiceDescriptor> tryAddService, IServiceCollection services, TKey key, ServiceDescriptor descriptor)
        {
            if (tryAddService == null) throw new ArgumentNullException(nameof(tryAddService));
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

            Type serviceType = descriptor.ServiceType;
            services.TryAddTransient(TypeBuilder<TKey>.IServiceWithKey(serviceType), TypeBuilder<TKey>.ServiceWithKey(serviceType));
            services.TryAddTransient(TypeBuilder<TKey>.IEnumerableKeyServicePair(serviceType), TypeBuilder<TKey>.EnumerableKeyServicePair(serviceType));
            services.TryAddTransient(TypeBuilder<TKey>.IEnumerableKeyLazyServicePair(serviceType), TypeBuilder<TKey>.EnumerableKeyLazyServicePair(serviceType));

            var proxyDescriptor = tryAddService(key, descriptor);
            if (proxyDescriptor != null)
            {
                // TODO: don't add twice & throw on inconsistent lifetime
                services.TryAdd(proxyDescriptor);
            }
        }
    }
}
