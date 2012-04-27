using System;

namespace Aec.Cqrs
{
    public interface IDocumentWriter<in TKey, TItem>
    {
        TItem AddOrUpdate(TKey key, Func<TItem> addFactory, Func<TItem, TItem> update);
        bool TryDelete(TKey key);
    }
}