using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest.RegisteredServices_2Class
{
    public sealed class TryAddServiceMethod
    {
        private interface IFoo { }
        private sealed class Foo : IFoo { }

        [Fact]
        public void IfTheSameKeyIsAddedSecondTime_ThenNullServiceDescriptorIsReturned()
        {
            // Arrange
            var registeredServices = new RegisteredServices<string, IFoo>();
            var transientDescriptor = ServiceDescriptor.Transient<IFoo, Foo>();
            var key = "key";
            registeredServices.TryAddService(key, transientDescriptor);

            // Act
            var serviceDescriptor = registeredServices.TryAddService(key, transientDescriptor);

            // Asset
            serviceDescriptor.Should().BeNull("the service with the same key has been already registered");
        }

        [Fact]
        public void IfImplementationInstanceIsProvided_ThenNullServiceDescriptorIsReturned()
        {
            // Arrange
            var registeredServices = new RegisteredServices<string, IFoo>();
            var singletonDescriptor = ServiceDescriptor.Singleton<IFoo>(new Foo());
            var key = "key";

            // Act
            var serviceDescriptor = registeredServices.TryAddService(key, singletonDescriptor);

            // Asset
            serviceDescriptor.Should().BeNull("the service with the same key has been already registered");
        }
    }
}
