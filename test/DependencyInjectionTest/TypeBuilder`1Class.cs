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

        [Theory, MemberData(nameof(Data))]
        public void IfParticularMethodOfTypeBuilderIsCalledWithTestServiceType_ThenCorrectTypeIsBuilt(Func<Type, Type> typeBuilderMethod, Type expectedType)
        {
            // Arrange
            var serviceType = typeof(IFoo);

            // Act
            Func<Type> act = () => typeBuilderMethod(serviceType);

            // Assert
            using (new AssertionScope())
            {
                Type result = act.Should().NotThrow().Subject;
                result.Should().Be(expectedType);
            }
        }

        public static TheoryData<Func<Type, Type>, Type> Data => new TheoryData<Func<Type, Type>, Type>
        {
            { TypeBuilder<string>.IServiceWithKey, typeof(IServiceWithKey<string, IFoo>) },
            { TypeBuilder<string>.ServiceWithKey, typeof(ServiceWithKey<string, IFoo>) },
            { TypeBuilder<string>.IEnumerableKeyServicePair, typeof(IEnumerable<KeyValuePair<string, IFoo>>) },
            { TypeBuilder<string>.EnumerableKeyServicePair, typeof(EnumerableKeyServicePair<string, IFoo>) },
            { TypeBuilder<string>.IEnumerableKeyLazyServicePair, typeof(IEnumerable<KeyValuePair<string, Lazy<IFoo>>>) },
            { TypeBuilder<string>.EnumerableKeyLazyServicePair, typeof(EnumerableKeyServicePair<string, Lazy<IFoo>>) },
            { TypeBuilder<string>.RegisteredServicesSingleton, typeof(RegisteredServicesSingleton<string, IFoo>) },
        };
    }
}
