namespace Msh.Microsoft.Extensions.DependencyInjection.Abstractions
{
    public interface IServiceWithKey<TKey, TService>
        where TService: class
    {
        TService this[TKey key] { get; }
    }
}
