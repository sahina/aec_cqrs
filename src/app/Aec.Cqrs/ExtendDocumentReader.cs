﻿using System;

namespace Aec.Cqrs
{
    public static class ExtendDocumentReader
    {
        public static TEntity Load<TKey, TEntity>(this IDocumentReader<TKey, TEntity> self, TKey key) where TKey : IIdentity
        {
            TEntity entity;
            if (self.TryGet(key, out entity))
            {
                return entity;
            }
            var txt = string.Format("Failed to load '{0}' with key '{1}'.", typeof(TEntity).Name, key);
            throw new InvalidOperationException(txt);
        }
    }
}
