using System;
using System.Collections;
using System.Collections.Generic;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class EnumerableKeyLazyServicesPair<TKey, TService> : IEnumerable<KeyValuePair<TKey, Lazy<TService>>>
    {
        private readonly IEnumerator<KeyValuePair<TKey, Lazy<TService>>> _enumerator;
        public EnumerableKeyLazyServicesPair(IServiceProvider serviceProvider) => _enumerator = RegisteredServicesSingleton<TKey, TService>.GetLazyServices(serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)));

        IEnumerator<KeyValuePair<TKey, Lazy<TService>>> IEnumerable<KeyValuePair<TKey, Lazy<TService>>>.GetEnumerator() => _enumerator;
        IEnumerator IEnumerable.GetEnumerator() => _enumerator;
    }
}
