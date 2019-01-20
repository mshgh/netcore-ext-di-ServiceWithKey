using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal static class TypeBuilder<TKey>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type IServiceWithKey(Type serviceType) => MakeGenericTypeWithKey(typeof(IServiceWithKey<,>), serviceType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type ServiceWithKey(Type serviceType) => MakeGenericTypeWithKey(typeof(ServiceWithKey<,>), serviceType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type IEnumerableKeyServicePair(Type serviceType) => typeof(IEnumerable<>).MakeGenericType(MakeGenericTypeWithKey(typeof(KeyValuePair<,>), serviceType));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type EnumerableKeyServicePair(Type serviceType) => MakeGenericTypeWithKey(typeof(EnumerableKeyServicePair<,>), serviceType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type IEnumerableKeyLazyServicePair(Type serviceType) => typeof(IEnumerable<>).MakeGenericType(MakeGenericTypeWithKey(typeof(KeyValuePair<,>), typeof(Lazy<>).MakeGenericType(serviceType)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type EnumerableKeyLazyServicePair(Type serviceType) => MakeGenericTypeWithKey(typeof(EnumerableKeyServicePair<,>), typeof(Lazy<>).MakeGenericType(serviceType));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type RegisteredServicesSingleton(Type serviceType) => MakeGenericTypeWithKey(typeof(RegisteredServicesSingleton<,>), serviceType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type MakeGenericTypeWithKey(Type keyedType, Type serviceType) => keyedType.MakeGenericType(typeof(TKey), serviceType);
    }
}
