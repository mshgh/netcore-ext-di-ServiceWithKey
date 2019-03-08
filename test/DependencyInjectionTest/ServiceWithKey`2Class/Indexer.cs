using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Msh.Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;
using Xunit;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest.ServiceWithKey_2Class
{
    public sealed class Indexer
    {
        public interface IFoo { }

        [Fact]
        public void IfServiceByNameIsRequested_ThenGetServiceFactoryIsCalledWithCorrectParams()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();
            var getServiceMock = new Mock<Func<string, IServiceProvider, IFoo>>();
            var serviceWithKey = new ServiceWithKey<string, IFoo>(serviceProviderMock.Object, getServiceMock.Object) as IServiceWithKey<string, IFoo>;
            const string key = "foo";

            // Act
            var service = serviceWithKey[key];

            // Assert
            using (new AssertionScope())
            {
                getServiceMock.Invocations.Count.Should().Be(1, "getService() method for given key should be called only once");
                getServiceMock.Invocations[0].Arguments[0].Should().Be("foo");
                getServiceMock.Invocations[0].Arguments[1].Should().BeSameAs(serviceProviderMock.Object);
            }
        }

        [Fact]
        public void IfServiceByNameIsRequestedMultipleTimes_ThenGetServiceFactoryIsCalledOlyOnce()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();
            var getServiceMock = new Mock<Func<string, IServiceProvider, IFoo>>();
            var serviceWithKey = new ServiceWithKey<string, IFoo>(serviceProviderMock.Object, getServiceMock.Object) as IServiceWithKey<string, IFoo>;
            const string key = "foo";

            // Act
            var service1 = serviceWithKey[key];
            var service2 = serviceWithKey[key];
            var service3 = serviceWithKey[key];

            // Assert
            using (new AssertionScope())
            {
                getServiceMock.Invocations.Count.Should().Be(1, "getService() method for given key should be called only once");
                getServiceMock.Invocations[0].Arguments[0].Should().Be("foo");
                getServiceMock.Invocations[0].Arguments[1].Should().BeSameAs(serviceProviderMock.Object);
                service2.Should().BeSameAs(service1, $"for particular key the same instance has to be returned everytime");
                service3.Should().BeSameAs(service1, $"for particular key the same instance has to be returned everytime");
            }
        }
    }
}
