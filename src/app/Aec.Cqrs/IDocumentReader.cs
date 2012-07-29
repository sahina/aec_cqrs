namespace Aec.Cqrs
{
    /// <summary>
    /// Reads document for views; used in projections
    /// </summary>
    /// <typeparam name="TKey">Type of document key</typeparam>
    /// <typeparam name="TItem">Type of document</typeparam>
    public interface IDocumentReader<in TKey, TItem> where TKey : IIdentity
    {
        /// <summary>
        /// Gets the item for the given key.
        /// </summary>
        /// <param name="key">Key of document</param>
        /// <param name="item">Item to return</param>
        /// <returns>Returns true if document can be returned, otherwise false</returns>
        bool TryGet(TKey key, out TItem item);
    }
}