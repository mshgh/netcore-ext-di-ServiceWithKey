namespace Msh.Microsoft.Extensions.DependencyInjection.Abstractions
{
    public interface IServiceWithKey<TKey, TService>
    {
        TService this[TKey key] { get; }
    }
}
