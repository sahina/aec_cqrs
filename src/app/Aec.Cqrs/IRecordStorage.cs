using System.Collections.Generic;

namespace Aec.Cqrs
{
    /// <summary>
    /// Record storage (message stream) interface.
    /// </summary>
    public interface IRecordStorage
    {
        /// <summary>
        /// Gets all the records for the given version and expected count.
        /// </summary>
        /// <param name="afterVersion">We get records after the given version number</param>
        /// <param name="maxCount">Number of records to fetch.</param>
        /// <returns>Collection of records.</returns>
        IEnumerable<SavedRecord> GetRecords(long afterVersion, int maxCount);
        
        /// <summary>
        /// Appends the given content to storage.
        /// </summary>
        /// <param name="content">Content to append to storage.</param>
        /// <returns>True if save is successfull, otherwise false.</returns>
        bool TryAppend(object[] content);
    }
}