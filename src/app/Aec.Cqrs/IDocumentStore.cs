using System.Collections.Generic;

namespace Aec.Cqrs
{
    /// <summary>
    /// Projection document storage
    /// </summary>
    public interface IDocumentStore
    {
        /// <summary>
        /// Gets document reader.
        /// </summary>
        /// <typeparam name="TKey">Type of document key</typeparam>
        /// <typeparam name="TItem">Type of document item</typeparam>
        /// <returns>Document reader</returns>
        IDocumentReader<TKey, TItem> GetReader<TKey, TItem>() where TKey : IIdentity;

        /// <summary>
        /// Gets document writer.
        /// </summary>
        /// <typeparam name="TKey">Type of document key</typeparam>
        /// <typeparam name="TItem">Type of document item</typeparam>
        /// <returns>Document writer</returns>
        IDocumentWriter<TKey, TItem> GetWriter<TKey, TItem>() where TKey : IIdentity;

        /// <summary>
        /// Gets the document strategy.
        /// </summary>
        IDocumentStrategy Strategy { get; }
        
        /// <summary>
        /// Resets document storage
        /// </summary>
        /// <param name="bucket">Name of bucket to reset. Bucket is used for partitioning.</param>
        void Reset(string bucket);
        
        /// <summary>
        /// Writes saved records
        /// </summary>
        /// <param name="bucket">Name of bucket to reset. Bucket is used for partitioning.</param>
        /// <param name="records">Records to write store</param>
        void WriteContents(string bucket, IEnumerable<DocumentRecord> records);

        /// <summary>
        /// Gets enumerable document records in the given bucket (partition)
        /// </summary>
        /// <param name="bucket">Bucket (partition)</param>
        /// <returns>List of document records</returns>
        IEnumerable<DocumentRecord> EnumerateContents(string bucket);
    }
}