using System;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class ServiceWithKey<TKey, TService> : IServiceWithKey<TKey, TService>
    {
        private readonly IServiceProvider _serviceProvider;
        public ServiceWithKey(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        #region interface IServiceWithKey
        TService IServiceWithKey<TKey, TService>.this[TKey key] => RegisteredServices<TKey, TService>.GetService(key, _serviceProvider);
        #endregion
    }
}
