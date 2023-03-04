namespace Webshop.Api.Interfaces
{
    public interface IUniqueEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
