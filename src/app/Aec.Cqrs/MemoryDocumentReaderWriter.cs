using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryDocumentReaderWriter<TKey, TItem> :
        IDocumentReader<TKey, TItem>,
        IDocumentWriter<TKey, TItem> where TKey : IIdentity
    {
        private readonly Dictionary<TKey, TItem> m_storage;

        public MemoryDocumentReaderWriter()
        {
            m_storage = new Dictionary<TKey, TItem>();
        }

        #region Implementation of IDocumentReader<in TKey,TEntity>

        public bool TryGet(TKey key, out TItem item)
        {
            return m_storage.TryGetValue(key, out item);
        }

        /// <summary>
        /// Gets all documents of given type.
        /// </summary>
        /// <param name="items">All documents of type.</param>
        /// <returns>Returns true if documents can be returned, otherwise false</returns>
        public bool TryGetAll(out IEnumerable<TItem> items)
        {
            var returned = true;
            items = new List<TItem>();

            try
            {
                items = m_storage.Values;
            }
            catch
            {
                returned = false;
            }
            
            return returned;
        }

        #endregion

        #region Implementation of IDocumentWriter<in TKey,TEntity>

        public TItem AddOrUpdate(TKey key, Func<TItem> addFactory, Func<TItem, TItem> update)
        {
            TItem result;

            if (m_storage.ContainsKey(key))
            {
                var entity = m_storage[key];

                result = update(entity);
            }
            else
            {
                result = addFactory();

                m_storage.Add(key, result);
            }

            return result;
        }

        public bool TryDelete(TKey key)
        {
            return m_storage.Remove(key);
        }

        #endregion
    }
}