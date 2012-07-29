using System;

namespace Aec.Cqrs
{
    /// <summary>
    /// Writes document in view; used in projections
    /// </summary>
    /// <typeparam name="TKey">Type of document key</typeparam>
    /// <typeparam name="TItem">Type of document</typeparam>
    public interface IDocumentWriter<in TKey, TItem> where TKey : IIdentity
    {
        /// <summary>
        /// Adds if entity is new, otherwise updates the given entity.
        /// </summary>
        /// <param name="key">Document key</param>
        /// <param name="addFactory">Add function</param>
        /// <param name="update">Update function</param>
        /// <returns>New or updated item</returns>
        TItem AddOrUpdate(TKey key, Func<TItem> addFactory, Func<TItem, TItem> update);
        
        /// <summary>
        /// Deletes given document.
        /// </summary>
        /// <param name="key">Key of document item to delete</param>
        /// <returns>True if item is deleted otherwise false</returns>
        bool TryDelete(TKey key);
    }
}