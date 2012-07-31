using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class DocumentStorage
    {
        private readonly IDocumentStore m_store;

        public DocumentStorage(IDocumentStore store)
        {
            m_store = store;
        }

        public void CopyFrom(DocumentStorage source, params string[] buckets)
        {
            foreach (var bucket in buckets)
            {
                m_store.WriteContents(bucket, source.m_store.EnumerateContents(bucket));
            }
        }

        public bool TryDeleteEntity<TEntity>(IIdentity key)
        {
            return m_store.GetWriter<IIdentity, TEntity>().TryDelete(key);
        }

        public TEntity UpdateEntity<TEntity>(IIdentity key, Action<TEntity> update)
        {
            return m_store.GetWriter<IIdentity, TEntity>().UpdateOrThrow(key, update);
        }

        public bool TryGetEntity<TEntity>(IIdentity key, out TEntity entity)
        {
            return m_store.GetReader<IIdentity, TEntity>().TryGet(key, out entity);
        }

        public bool TryGetAllEntities<TEntity>(out IEnumerable<TEntity> entities)
        {
            return m_store.GetReader<IIdentity, TEntity>().TryGetAll(out entities);
        }

        public TEntity AddOrUpdateEntity<TEntity>(IIdentity key, TEntity entity)
        {
            return m_store.GetWriter<IIdentity, TEntity>().AddOrUpdate(key, () => entity, source => entity);
        }

        public TEntity AddOrUpdateEntity<TEntity>(IIdentity key, Func<TEntity> addFactory, Action<TEntity> update)
        {
            return m_store.GetWriter<IIdentity, TEntity>().AddOrUpdate(key, addFactory, update);
        }

        public TEntity AddOrUpdateEntity<TEntity>(IIdentity key, Func<TEntity> addFactory, Func<TEntity, TEntity> update)
        {
            return m_store.GetWriter<IIdentity, TEntity>().AddOrUpdate(key, addFactory, update);
        }

        public TEntity AddEntity<TEntity>(IIdentity key, TEntity newEntity)
        {
            return m_store.GetWriter<IIdentity, TEntity>().Add(key, newEntity);
        }
    }
}
