using System;
using System.Collections.Generic;
using System.Linq;

namespace Aec.Cqrs
{
    public class MemoryRecordStorage : IRecordStorage
    {
        private readonly IIdentity m_id;
        private readonly HashSet<SavedRecord> m_storage;

        public MemoryRecordStorage(IIdentity id)
        {
            m_id = id;
            m_storage = new HashSet<SavedRecord>();
        }

        #region Implementation of IRecordStorage

        public IEnumerable<SavedRecord> GetRecords(long afterVersion, int maxCount)
        {
            if (afterVersion < 0)
                throw new ArgumentOutOfRangeException("afterVersion", "Must be zero or greater.");

            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException("maxCount", "Must be more than zero.");

            // afterVersion + maxCount > long.MaxValue, but transformed to avoid overflow
            if (afterVersion > long.MaxValue - maxCount)
                throw new ArgumentOutOfRangeException("maxCount", "Version will exceed long.MaxValue.");

            var records = m_storage
                .Skip((int)afterVersion)
                .Take(maxCount)
                .ToArray();

            return records;
        }

        public bool TryAppend(object[] content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            try
            {
                var lastVersion = m_storage.Any() ? m_storage.Last().Version : 0;

                content.ToList().ForEach(m =>
                {
                    var version = ++lastVersion;
                    var key = string.Format("{0}-{1:00000000}", m.GetType().Name, version);

                    var savedMessage = new SavedRecord(key, version, m);

                    m_storage.Add(savedMessage);
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}