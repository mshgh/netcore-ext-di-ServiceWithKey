using System;
using FluentAssertions;
using FluentAssertions.Execution;
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
        public void IfTheSameKeyIsAddedMultipleTimes_ThenOnlyFirstCallReturnsAdjustedServiceDescriptorAndSubsequentCallsRetunNull()
        {
            // Arrange
            var registeredServices = new RegisteredServices<string, IFoo>();
            var transientDescriptor = ServiceDescriptor.Transient<IFoo, Foo>();
            var key = "key";

            // Act
            Func<(ServiceDescriptor, ServiceDescriptor, ServiceDescriptor)> act = () =>
            (
                registeredServices.TryAddService(key, transientDescriptor),
                registeredServices.TryAddService(key, transientDescriptor),
                registeredServices.TryAddService(key, transientDescriptor)
            );

            // Asset
            using (new AssertionScope())
            {
                var (result1, result2, result3) = act.Should().NotThrow().Subject;
                result1.Should().BeEquivalentTo(ServiceDescriptor.Transient<Foo, Foo>());
                result2.Should().BeNull();
                result3.Should().BeNull();
            }
        }

        [Fact]
        public void IfServiceDescriptorWithImplementationInstanceIsProvided_ThenNullIsReturned()
        {
            // Arrange
            var registeredServices = new RegisteredServices<string, IFoo>();
            var singletonDescriptor = ServiceDescriptor.Singleton<IFoo>(new Foo());
            var key = "key";

            // Act
            Func<ServiceDescriptor> act = () => registeredServices.TryAddService(key, singletonDescriptor);

            // Asset
            var result = act.Should().NotThrow().Subject;
            result.Should().BeNull();
        }
    }
}
