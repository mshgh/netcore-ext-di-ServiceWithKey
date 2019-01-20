using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal class RegisteredServices<TKey, TService>
    {
        private readonly IDictionary<TKey, Implementation> _cache = new Dictionary<TKey, Implementation>();

        public ServiceDescriptor TryAddService(TKey key, ServiceDescriptor serviceDescriptor)
        {
            if (_cache.ContainsKey(key)) return null;

            if (serviceDescriptor.ImplementationInstance != null)
            {
                _cache.Add(key, new Implementation { Instance = (TService)serviceDescriptor.ImplementationInstance });
                return null;
            }

            Type type = serviceDescriptor.ImplementationType;
            if (type == null) // this must be 'ImplementationFactory'
            {
                type = typeof(ServiceFactoryProxy<>).MakeGenericType(ClassGenerator.GetUniqueClassType());
                var setServiceFactory = DelegateHelper.GetSetServiceFactory(type);
                setServiceFactory(serviceDescriptor.ImplementationFactory);
            }

            _cache.Add(key, new Implementation { Type = type });
            return new ServiceDescriptor(type, type, serviceDescriptor.Lifetime);
        }

        public TService GetService(TKey key, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (!_cache.TryGetValue(key, out var implementation)) throw new Exception($"There is no service with key '{key}' available");

            TService service = implementation.Instance;
            if (service == null)
            {
                var instance = serviceProvider.GetService(implementation.Type);
                var proxy = instance as ServiceFactoryProxy;
                service = proxy != null ? proxy.Service : (TService)instance;
            }

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

        #region nested classes
        #region nested class Implementation
        private sealed class Implementation
        {
            public TService Instance;
            public Type Type;
        }
        #endregion

        #region nested  class ServiceFactoryProxy
        private abstract class ServiceFactoryProxy
        {
            private readonly Lazy<TService> _service;

            public ServiceFactoryProxy(IServiceProvider serviceProvider, Func<IServiceProvider, object> serviceFactory)
            {
                if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
                if (serviceFactory == null) throw new ArgumentNullException(nameof(serviceFactory));

                _service = new Lazy<TService>(() => (TService)serviceFactory(serviceProvider));
            }

            public TService Service => _service.Value;
        }
        #endregion

        #region nested class ServiceFactoryProxy<TUnique>
        private sealed class ServiceFactoryProxy<TUnique> : ServiceFactoryProxy
        {
            private static Func<IServiceProvider, object> _serviceFactory;

            public ServiceFactoryProxy(IServiceProvider serviceProvider) : base(serviceProvider, _serviceFactory) { }

            public static void SetServiceFactory(Func<IServiceProvider, object> serviceFactory) => _serviceFactory = serviceFactory;
        }
        #endregion

        #region nested class ClassGenerator
        private static class ClassGenerator
        {
            private static readonly IReadOnlyList<Type> HelperTypes = new[] {
            typeof(C0), typeof(C0.Ca), typeof(C0.Cb), typeof(C0.Cc),
            typeof(C1), typeof(C1.Ca), typeof(C1.Cb), typeof(C1.Cc),
            typeof(C2), typeof(C2.Ca), typeof(C2.Cb), typeof(C2.Cc),
            typeof(C3), typeof(C3.Ca), typeof(C3.Cb), typeof(C3.Cc),
        };

            private static int _sequence = -1;

            public static Type GetUniqueClassType()
            {
                var sequence = Interlocked.Increment(ref _sequence);

                var types = new Type[8]; // there is 8 nibbles per 32bit int
                for (int index = 0; index < types.Length; ++index)
                {
                    types[index] = HelperTypes[sequence & 0xF];
                    sequence >>= 4;
                }

                return typeof(UniqueClass<,,,,,,,>).MakeGenericType(types);
            }

            private sealed class UniqueClass<TN1, TN2, TN3, TN4, TN5, TN6, TN7, TN8> { }

            #region pool of 16 helper classes
            private sealed class C0 { public sealed class Ca { } public sealed class Cb { } public sealed class Cc { } }
            private sealed class C1 { public sealed class Ca { } public sealed class Cb { } public sealed class Cc { } }
            private sealed class C2 { public sealed class Ca { } public sealed class Cb { } public sealed class Cc { } }
            private sealed class C3 { public sealed class Ca { } public sealed class Cb { } public sealed class Cc { } }
            #endregion
        }
        #endregion
        #endregion
    }
}
