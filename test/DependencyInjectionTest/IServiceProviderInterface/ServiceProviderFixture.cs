using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Msh.Microsoft.Extensions.DependencyInjection;

namespace Msh.Microsoft.Extensions.Tests.DependencyInjectionTest.IServiceProviderInterface
{
    public sealed class ServiceProviderFixture : IDisposable
    {
        public IServiceProvider CompleteServiceProvider { get; } = new ServiceCollection()
            .TryAddTransient<string, ITransient, TransientA>("A")
            .TryAddTransient<string, ITransient, TransientB>("B")
            .BuildServiceProvider();

        void IDisposable.Dispose() => new[] { CompleteServiceProvider }
            .OfType<IDisposable>().ToList().ForEach(d => d.Dispose());

        public interface ITransient { }
        public sealed class TransientA : ITransient { }
        public sealed class TransientB : ITransient { }

        public interface IScoped { }
        public sealed class ScopedA : IScoped { }
        public sealed class ScopedB : IScoped { }

        public interface ISingleton { }
        public sealed class SingletonA : ISingleton { }
        public sealed class SingletonB : ISingleton { }
    }
}
