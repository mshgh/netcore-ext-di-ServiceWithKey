using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal static class DelegateHelper
    {
        private static readonly ConcurrentDictionary<(Type, string), object> _delegates = new ConcurrentDictionary<(Type, string), object>();

        public static Func<TKey, ServiceDescriptor, ServiceDescriptor> GetTryAddServiceOfRegisteredServicesSingleton<TKey>(Type serviceType)
            => GetDelegate<Func<TKey, ServiceDescriptor, ServiceDescriptor>>(TypeBuilder<TKey>.RegisteredServicesSingleton(serviceType), "TryAddService");

        public static Action<Func<IServiceProvider, object>> GetSetServiceFactory(Type serviceFactoryProxy)
            => GetDelegate<Action<Func<IServiceProvider, object>>>(serviceFactoryProxy, "SetServiceFactory");

        private static TDelegate GetDelegate<TDelegate>(Type type, string name) => (TDelegate)_delegates.GetOrAdd((type, name),
            key =>
            {
                (Type classType, string methodName) = key;

                if (classType == null) throw new ArgumentNullException($"{nameof(classType)}");
                if (string.IsNullOrEmpty(methodName)) throw new ArgumentException("Method name must not be null or empty", $"{nameof(methodName)}");

                var methodInfo = classType.GetMethod(methodName);
                if (methodInfo == null) throw new Exception($"Cannot find method '{methodName}' in type '{classType}'");

                return methodInfo.CreateDelegate(typeof(TDelegate));
            });
    }
}
