namespace Domain.Entities.Common
{
    /// Ref: Coverience in C#
    /// <summary>
    /// Interface for Base Entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseEntity<out TKey>
    {
        TKey Id { get; }
    }
}
