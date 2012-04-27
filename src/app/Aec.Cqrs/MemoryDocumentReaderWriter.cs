using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryDocumentReaderWriter<TKey, TItem> : IDocumentReader<TKey, TItem>, IDocumentWriter<TKey, TItem>
    {
        public Dictionary<TKey, TItem> Storage { get; set; }

        public MemoryDocumentReaderWriter()
        {
            Storage = new Dictionary<TKey, TItem>();
        }

        #region Implementation of IDocumentReader<in TKey,TEntity>

        public bool TryGet(TKey key, out TItem item)
        {
            return Storage.TryGetValue(key, out item);
        }

        #endregion

        #region Implementation of IDocumentWriter<in TKey,TEntity>

        public TItem AddOrUpdate(TKey key, Func<TItem> addFactory, Func<TItem, TItem> update)
        {
            TItem result;

            if (Storage.ContainsKey(key))
            {
                var entity = Storage[key];

                result = update(entity);
            }
            else
            {
                result = addFactory();

                Storage.Add(key, result);
            }

            return result;
        }

        public bool TryDelete(TKey key)
        {
            return Storage.Remove(key);
        }

        #endregion
    }
}