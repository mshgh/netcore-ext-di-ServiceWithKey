using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal static class RegisteredServicesSingleton<TKey, TService>
    {
        private static readonly RegisteredServices<TKey, TService> _instance = new RegisteredServices<TKey, TService>();

        public static ServiceDescriptor TryAddService(TKey key, ServiceDescriptor descriptor) => _instance.TryAddService(key, descriptor);
        public static TService GetService(TKey key, IServiceProvider serviceProvider) => _instance.GetService(key, serviceProvider);
        public static IEnumerator<KeyValuePair<TKey, TService>> GetServices(IServiceProvider serviceProvider) => _instance.GetServices(serviceProvider);
        public static IEnumerator<KeyValuePair<TKey, Lazy<TService>>> GetLazyServices(IServiceProvider serviceProvider) => _instance.GetLazyServices(serviceProvider);
    }
}
