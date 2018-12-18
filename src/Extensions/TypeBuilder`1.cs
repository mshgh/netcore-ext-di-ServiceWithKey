using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal static class TypeBuilder<TKey>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type IServiceWithKey(ServiceDescriptor descriptor) => MakeKeyServiceType(typeof(IServiceWithKey<,>), descriptor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type ServiceWithKey(ServiceDescriptor descriptor) => MakeKeyServiceType(typeof(ServiceWithKey<,>), descriptor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type IEnumerableKeyValuePair(ServiceDescriptor descriptor) => typeof(IEnumerable<>).MakeGenericType(MakeKeyServiceType(typeof(KeyValuePair<,>), descriptor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type ServicesWithKey(ServiceDescriptor descriptor) => MakeKeyServiceType(typeof(ServicesWithKey<,>), descriptor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type RegisteredServices(ServiceDescriptor descriptor) => MakeKeyServiceType(typeof(RegisteredServices<,>), descriptor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type MakeKeyServiceType(Type type, ServiceDescriptor descriptor) => type.MakeGenericType(typeof(TKey), descriptor.ServiceType);
    }
}
