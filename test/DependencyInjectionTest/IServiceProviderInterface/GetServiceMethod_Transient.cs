﻿using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection.Abstractions;
using Xunit;
using static Msh.Microsoft.Extensions.Tests.DependencyInjectionTest.IServiceProviderInterface.ServiceProviderFixture;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest.IServiceProviderInterface
{
    public sealed class GetServiceMethod_Transient : IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;

        public GetServiceMethod_Transient(ServiceProviderFixture serviceProviderFixture)
        {
            _serviceProviderFixture = serviceProviderFixture ?? throw new ArgumentNullException(nameof(serviceProviderFixture));
        }

        [Fact]
        public void IfTransientServiceIsResolvedTwiceWithTheSameKey_ThenNewInstanceIsCreatedEveryTime()
        {
            // Arrange
            var serviceProvider = _serviceProviderFixture.CompleteServiceProvider;

            // Act
            Func<(ITransient, ITransient)> act = () => (
              serviceProvider.GetService<IServiceWithKey<string, ITransient>>()["A"],
              serviceProvider.GetService<IServiceWithKey<string, ITransient>>()["A"]
          );

            // Asset
            using (new AssertionScope())
            {
                var (instanceOne, instanceTwo) = act.Should().NotThrow().Subject;
                instanceOne.Should().NotBeNull().And.BeOfType<TransientA>();
                instanceTwo.Should().NotBeNull().And.BeOfType<TransientA>().And.NotBeSameAs(instanceOne);
            }
        }

        [Fact]
        public void IfTransientServiceIsResolvedTwiceWithDifferentKeys_ThenInstanceOfAppropriateTypeIsCreated()
        {
            // Arrange
            var serviceProvider = _serviceProviderFixture.CompleteServiceProvider;

            // Act
            Func<(ITransient, ITransient)> act = () => (
              serviceProvider.GetService<IServiceWithKey<string, ITransient>>()["A"],
              serviceProvider.GetService<IServiceWithKey<string, ITransient>>()["B"]
          );

            // Asset
            using (new AssertionScope())
            {
                var (instanceOne, instanceTwo) = act.Should().NotThrow().Subject;
                instanceOne.Should().NotBeNull().And.BeOfType<TransientA>();
                instanceTwo.Should().NotBeNull().And.BeOfType<TransientB>();
            }
        }
    }
}
