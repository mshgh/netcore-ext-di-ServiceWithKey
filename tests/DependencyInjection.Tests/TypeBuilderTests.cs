using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;
using Xunit;

namespace Msh.Microsoft.Extensions.DependencyInjection.Tests
{
    public sealed class TypeBuilderTests
    {
        private interface IFoo { }
        private sealed class Foo : IFoo { }

        [Fact]
        public void GenericType_IServiceWithKey_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(IServiceWithKey<string, IFoo>), TypeBuilder<string>.IServiceWithKey);

        [Fact]
        public void GenericType_ServiceWithKey_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(ServiceWithKey<string, IFoo>), TypeBuilder<string>.ServiceWithKey);

        [Fact]
        public void GenericType_IEnumerableKeyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(IEnumerable<KeyValuePair<string, IFoo>>), TypeBuilder<string>.IEnumerableKeyServicePair);

        [Fact]
        public void GenericType_EnumerableKeyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(EnumerableKeyServicePair<string, IFoo>), TypeBuilder<string>.EnumerableKeyServicePair);

        [Fact]
        public void GenericType_IEnumerableKeyLazyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(IEnumerable<KeyValuePair<string, Lazy<IFoo>>>), TypeBuilder<string>.IEnumerableKeyLazyServicePair);

        [Fact]
        public void GenericType_EnumerableKeyLazyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(EnumerableKeyServicePair<string, Lazy<IFoo>>), TypeBuilder<string>.EnumerableKeyLazyServicePair);

        [Fact]
        public void GenericType_RegisteredServicesSingleton_IsBuiltCorrectly() => TestTypeBuilder<IFoo, Foo>(typeof(RegisteredServicesSingleton<string, IFoo>), TypeBuilder<string>.RegisteredServicesSingleton);

        private static void TestTypeBuilder<TService, TImplementation>(Type expectedType, Func<Type, Type> buildType)
            where TService : class
            where TImplementation : class, TService
        {
            // Arrange
            var descriptor = ServiceDescriptor.Transient<TService, TImplementation>();

            // Act
            Func<Type> act = () => buildType(descriptor.ServiceType);

            // Assert
            using (new AssertionScope())
            {
                var reasult = act.Should().NotThrow().Subject;
                reasult.Should().Be(expectedType);
            }
        }
    }
}
