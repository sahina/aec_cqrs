using System.Collections.Generic;

namespace Aec.Cqrs
{
    /// <summary>
    /// Projection document storage
    /// </summary>
    public interface IDocumentStorage
    {
        /// <summary>
        /// Gets document reader.
        /// </summary>
        /// <typeparam name="TKey">Type of document key</typeparam>
        /// <typeparam name="TItem">Type of document item</typeparam>
        /// <returns>Document reader</returns>
        IDocumentReader<TKey, TItem> GetReader<TKey, TItem>();

        /// <summary>
        /// Gets document writer.
        /// </summary>
        /// <typeparam name="TKey">Type of document key</typeparam>
        /// <typeparam name="TItem">Type of document item</typeparam>
        /// <returns>Document writer</returns>
        IDocumentWriter<TKey, TItem> GetWriter<TKey, TItem>();
        
        /// <summary>
        /// Resets document storage
        /// </summary>
        void Reset();
        
        /// <summary>
        /// Writes saved records
        /// </summary>
        /// <param name="records"></param>
        void WriteContents(IEnumerable<SavedRecord> records);
    }
}