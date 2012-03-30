using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryAtomicReaderWriter<TKey, TItem> : IAtomicReader<TKey, TItem>, IAtomicWriter<TKey, TItem>
    {
        public Dictionary<TKey, TItem> Storage { get; set; }

        public MemoryAtomicReaderWriter()
        {
            Storage = new Dictionary<TKey, TItem>();
        }

        #region Implementation of IAtomicReader<in TKey,TEntity>

        public bool TryGet(TKey key, out TItem item)
        {
            return Storage.TryGetValue(key, out item);
        }

        #endregion

        #region Implementation of IAtomicWriter<in TKey,TEntity>

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