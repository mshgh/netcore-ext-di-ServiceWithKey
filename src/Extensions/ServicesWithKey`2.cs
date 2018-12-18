using System;
using System.Collections;
using System.Collections.Generic;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class ServicesWithKey<TKey, TService> : IEnumerable<KeyValuePair<TKey, TService>>
    {
        private readonly IEnumerator<KeyValuePair<TKey, TService>> _enumerator;
        public ServicesWithKey(IServiceProvider serviceProvider) => _enumerator = RegisteredServices<TKey, TService>.GetServices(serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)));

        IEnumerator<KeyValuePair<TKey, TService>> IEnumerable<KeyValuePair<TKey, TService>>.GetEnumerator() => _enumerator;
        IEnumerator IEnumerable.GetEnumerator() => _enumerator;
    }
}
