using System;
using System.Collections.Concurrent;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal sealed class DelegateHelper
    {
        private static readonly ConcurrentDictionary<(Type, string), object> _delegates = new ConcurrentDictionary<(Type, string), object>();

        public static TDelegate GetDelegate<TDelegate>(Type type, string name) => (TDelegate)_delegates.GetOrAdd((type, name), CreateDelegate<TDelegate>);

        private static object CreateDelegate<TDelegate>((Type Type, string MethodName) key)
        {
            if (key.Type == null) throw new ArgumentNullException($"{nameof(key)}.{nameof(key.Type)}");
            if (string.IsNullOrEmpty(key.MethodName)) throw new ArgumentNullException($"{nameof(key)}.{nameof(key.MethodName)}");

            var methodInfo = key.Type.GetMethod(key.MethodName);
            if (methodInfo == null) throw new Exception($"Cannot find method '{key.MethodName}' for type '{key.Type}'");
            
            return methodInfo.CreateDelegate(typeof(TDelegate));
        }
    }
}
