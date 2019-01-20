using System;
using System.Collections.Concurrent;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class ServiceWithKey<TKey, TService> : IServiceWithKey<TKey, TService>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TService>> _services = new ConcurrentDictionary<TKey, Lazy<TService>>();
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<TKey, IServiceProvider, TService> _getService;

        public ServiceWithKey(IServiceProvider serviceProvider) : this(serviceProvider, RegisteredServicesSingleton<TKey, TService>.GetService) { }
        internal ServiceWithKey(IServiceProvider serviceProvider, Func<TKey, IServiceProvider, TService> getService)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _getService = getService ?? throw new ArgumentNullException(nameof(getService));
        }

        #region interface IServiceWithKey
        TService IServiceWithKey<TKey, TService>.this[TKey key] => _services.GetOrAdd(key, k => new Lazy<TService>(() => _getService(k, _serviceProvider))).Value;
        #endregion
    }
}
