namespace Aec.Cqrs
{
    public interface IAtomicReader<in TKey, TItem>
    {
        bool TryGet(TKey key, out TItem item);
    }
}