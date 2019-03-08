using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Msh.Microsoft.Extensions.DependencyInjection
{
    internal abstract class ServiceProxy
    {
        public ServiceProxy(object service) => Service = service ?? throw new ArgumentNullException(nameof(service));

        public object Service { get; }

        public static Type WrapImplementationFactory<TKey>(Func<IServiceProvider, object> implementationFactory)
        {
            Type implementationType = ServiceProxyBuilder<TKey>.CreateUniqueProxyType();
            Action<Func<IServiceProvider, object>> setImplementationFactory = DelegateHelper.GetSetImplementationFactoryMethod(implementationType);
            setImplementationFactory(implementationFactory);
            return implementationType;
        }

        #region nested class ServiceProxyBuilder<TKey>
        private static class ServiceProxyBuilder<TKey>
        {
            private static int _sequence = -1;

            public static Type CreateUniqueProxyType()
            {
                int sequence = Interlocked.Increment(ref _sequence);
                Type[] types = Enumerable.Range(0, 8).Select(idx => TypesPool[GetNibble(sequence, idx)]).ToArray();
                return typeof(IntegerValueClass<,,,,,,,>).MakeGenericType(types);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int GetNibble(int value, int index) => (value >> (4 * index)) & 0x0F;

            #region nested class ServiceProxy<TUnique>
            private sealed class ServiceProxy<TUnique> : ServiceProxy
            {
                private static Func<IServiceProvider, object> _implementationFactory;

                public ServiceProxy(IServiceProvider serviceProvider) : base(GetService(serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)))) { }

                public static void SetImplementationFactory(Func<IServiceProvider, object> implementationFactory) => _implementationFactory = implementationFactory ?? throw new ArgumentNullException(nameof(implementationFactory));

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static object GetService(IServiceProvider serviceProvider) => (_implementationFactory ?? throw new ArgumentNullException(nameof(_implementationFactory)))(serviceProvider);
            }
            #endregion

            #region helper classes to turn integer value into class type
            private static readonly IReadOnlyList<Type> TypesPool = new[]
            {
                typeof(C0), typeof(C0.Ca), typeof(C0.Cb), typeof(C0.Cc),
                typeof(C1), typeof(C1.Ca), typeof(C1.Cb), typeof(C1.Cc),
                typeof(C2), typeof(C2.Ca), typeof(C2.Cb), typeof(C2.Cc),
                typeof(C3), typeof(C3.Ca), typeof(C3.Cb), typeof(C3.Cc),
            };

            private static class IntegerValueClass<TN1, TN2, TN3, TN4, TN5, TN6, TN7, TN8> { }

            private static class C0 { public static class Ca { } public static class Cb { } public static class Cc { } }
            private static class C1 { public static class Ca { } public static class Cb { } public static class Cc { } }
            private static class C2 { public static class Ca { } public static class Cb { } public static class Cc { } }
            private static class C3 { public static class Ca { } public static class Cb { } public static class Cc { } }
            #endregion
        }
        #endregion
    }
}
