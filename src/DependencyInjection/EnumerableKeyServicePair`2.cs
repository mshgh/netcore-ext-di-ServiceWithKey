using System;
using System.Collections;
using System.Collections.Generic;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class EnumerableKeyServicePair<TKey, TService> : IEnumerable<KeyValuePair<TKey, TService>>
        where TService: class
    {
        private readonly IEnumerator<KeyValuePair<TKey, TService>> _enumerator;
        public EnumerableKeyServicePair(IServiceProvider serviceProvider) => _enumerator = RegisteredServicesSingleton<TKey, TService>.GetServices(serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)));

        IEnumerator<KeyValuePair<TKey, TService>> IEnumerable<KeyValuePair<TKey, TService>>.GetEnumerator() => _enumerator;
        IEnumerator IEnumerable.GetEnumerator() => _enumerator;
    }
}
