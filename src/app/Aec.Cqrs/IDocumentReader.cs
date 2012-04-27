namespace Aec.Cqrs
{
    public interface IDocumentReader<in TKey, TItem>
    {
        bool TryGet(TKey key, out TItem item);
    }
}