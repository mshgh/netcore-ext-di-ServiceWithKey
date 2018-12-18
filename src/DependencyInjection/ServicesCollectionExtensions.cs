using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
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

        public static IServiceCollection TryAddSingleton<TKey, TService, TImplementation>(this IServiceCollection services, TKey key)
            where TService : class
            where TImplementation : class, TService
        {
            return services.TryAdd(key, ServiceDescriptor.Singleton<TService, TImplementation>());
        }

        // TODO: all overwrites for Transient/Scoped/Singleton

        public static IServiceCollection TryAdd<TKey>(this IServiceCollection services, TKey key, ServiceDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

            services.TryAddSingleton(TypeBuilder<TKey>.IServiceWithKey(descriptor), TypeBuilder<TKey>.ServiceWithKey(descriptor));
            services.TryAddTransient(TypeBuilder<TKey>.IEnumerableKeyValuePair(descriptor), TypeBuilder<TKey>.ServicesWithKey(descriptor));

            var tryAddService = DelegateHelper.GetDelegate<Func<TKey, ServiceDescriptor, ServiceDescriptor>>(TypeBuilder<TKey>.RegisteredServices(descriptor), "TryAddService");
            var proxyDescriptor = tryAddService(key, descriptor);
            if (proxyDescriptor != null) services.TryAdd(proxyDescriptor);

            return services;
        }
    }
}
