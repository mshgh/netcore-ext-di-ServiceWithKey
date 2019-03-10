using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest
{
    public sealed class DelegateHelperClass
    {
        private interface IFoo { }
        private static class DummyServiceProxy
        {
            public static void SetImplementationFactory(Func<IServiceProvider, object> implementationFactory) { }
        }

        [Fact]
        public void IfGetTryAddServiceMethodOfRegisteredServicesSingletonIsCalledWithoutParameter_ThenExceptionIsThrown()
        {
            // Arrange
            Type serviceType = null;

            // Act
            Func<Func<string, ServiceDescriptor, ServiceDescriptor>> act = () => DelegateHelper.GetTryAddServiceMethodOfRegisteredServicesSingleton<string>(serviceType);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void IfGetTryAddServiceMethodOfRegisteredServicesSingletonIsCalledWithServiceType_ThenDelegateIsReturned()
        {
            // Arrange
            Type serviceType = typeof(IFoo);

            // Act
            Func<Func<string, ServiceDescriptor, ServiceDescriptor>> act = () => DelegateHelper.GetTryAddServiceMethodOfRegisteredServicesSingleton<string>(serviceType);

            // Assert
            var result = act.Should().NotThrow().Subject;
            result.Should().NotBeNull();
        }

        [Fact]
        public void IfGetSetImplementationFactoryMethodIsCalledWithoutParameter_ThenExceptionIsThrown()
        {
            // Arrange
            Type serviceProxy = null;

            // Act
            Func<Action<Func<IServiceProvider, object>>> act = () => DelegateHelper.GetSetImplementationFactoryMethod(serviceProxy);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void IfGetSetImplementationFactoryMethodIsCalledWithServiceType_ThenDelegateIsReturned()
        {
            // Arrange
            Type serviceProxy = typeof(DummyServiceProxy);

            // Act
            Func<Action<Func<IServiceProvider, object>>> act = () => DelegateHelper.GetSetImplementationFactoryMethod(serviceProxy);

            // Assert
            var result = act.Should().NotThrow().Subject;
            result.Should().NotBeNull();
        }
    }
}
