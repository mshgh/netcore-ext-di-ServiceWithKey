using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal class RegisteredServices<TKey, TService>
        where TService: class
    {
        private readonly IDictionary<TKey, Descriptor> _cache = new Dictionary<TKey, Descriptor>();

        public ServiceDescriptor TryAddService(TKey key, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor == null) throw new ArgumentNullException(nameof(serviceDescriptor));

            if (_cache.ContainsKey(key)) return null;

            if (serviceDescriptor.ImplementationInstance != null)
            {
                _cache.Add(key, new Descriptor { Instance = (TService)serviceDescriptor.ImplementationInstance });
                return null;
            }

            Type implementationType = serviceDescriptor.ImplementationType ?? ServiceProxy.WrapImplementationFactory<TKey>(serviceDescriptor.ImplementationFactory);
            _cache.Add(key, new Descriptor { Type = implementationType });

            return new ServiceDescriptor(implementationType, implementationType, serviceDescriptor.Lifetime);
        }

        public TService GetService(TKey key, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (!_cache.TryGetValue(key, out Descriptor descriptor)) throw new Exception($"There is no service '{typeof(TService).FullName}' with key '{key}' available");

            TService service = descriptor.Instance ?? UnwrapService(serviceProvider.GetService(descriptor.Type));
            return service;
        }

        public IEnumerator<KeyValuePair<TKey, TService>> GetServices(IServiceProvider serviceProvider)
        {
            foreach (var key in _cache.Keys)
            {
                yield return new KeyValuePair<TKey, TService>(key, GetService(key, serviceProvider));
            }
        }

        public IEnumerator<KeyValuePair<TKey, Lazy<TService>>> GetLazyServices(IServiceProvider serviceProvider)
        {
            foreach (var key in _cache.Keys)
            {
                yield return new KeyValuePair<TKey, Lazy<TService>>(key, new Lazy<TService>(() => GetService(key, serviceProvider)));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TService UnwrapService(object instance) => (TService)(instance is ServiceProxy proxy ? proxy.Service : instance);

        #region nested class Descriptor
        private sealed class Descriptor
        {
            public TService Instance;
            public Type Type;
        }
        #endregion
    }
}
