using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Msh.Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;
using Xunit;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest
{
    public sealed class TypeBuilder_1Class
    {
        private interface IFoo { }

        [Fact]
        public void GenericType_IServiceWithKey_IsBuiltCorrectly() => TestTypeBuilder<IServiceWithKey<string, IFoo>>(TypeBuilder<string>.IServiceWithKey);

        [Fact]
        public void GenericType_ServiceWithKey_IsBuiltCorrectly() => TestTypeBuilder<ServiceWithKey<string, IFoo>>(TypeBuilder<string>.ServiceWithKey);

        [Fact]
        public void GenericType_IEnumerableKeyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IEnumerable<KeyValuePair<string, IFoo>>>(TypeBuilder<string>.IEnumerableKeyServicePair);

        [Fact]
        public void GenericType_EnumerableKeyServicePair_IsBuiltCorrectly() => TestTypeBuilder<EnumerableKeyServicePair<string, IFoo>>(TypeBuilder<string>.EnumerableKeyServicePair);

        [Fact]
        public void GenericType_IEnumerableKeyLazyServicePair_IsBuiltCorrectly() => TestTypeBuilder<IEnumerable<KeyValuePair<string, Lazy<IFoo>>>>(TypeBuilder<string>.IEnumerableKeyLazyServicePair);

        [Fact]
        public void GenericType_EnumerableKeyLazyServicePair_IsBuiltCorrectly() => TestTypeBuilder<EnumerableKeyServicePair<string, Lazy<IFoo>>>(TypeBuilder<string>.EnumerableKeyLazyServicePair);

        [Fact]
        public void GenericType_RegisteredServicesSingleton_IsBuiltCorrectly() => TestTypeBuilder(typeof(RegisteredServicesSingleton<string, IFoo>), TypeBuilder<string>.RegisteredServicesSingleton);

        private static void TestTypeBuilder<TExpected>(Func<Type, Type> buildType) => TestTypeBuilder(typeof(TExpected), buildType);
        private static void TestTypeBuilder(Type expectedType, Func<Type, Type> buildType)
        {
            // Arrange
            var serviceType = typeof(IFoo);

            // Act
            Func<Type> act = () => buildType(serviceType);

            // Assert
            using (new AssertionScope())
            {
                var reasult = act.Should().NotThrow().Subject;
                reasult.Should().Be(expectedType);
            }
        }
    }
}
