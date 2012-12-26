using System;
using System.Collections.Generic;
using System.Linq;
using Aec.Infrastructure.Framework.Logging;
using EventStore;

namespace Aec.Cqrs.EventStorage
{
    public class EventStoreRecordStorage : IRecordStorage
    {
        private readonly Guid m_streamID;
        private readonly IIdentity m_identity;
        private readonly IStoreEvents m_storage;
        private readonly ILogger m_logger;

        public EventStoreRecordStorage(IIdentity identity, IStoreEvents storage, ILogger logger)
        {
            if (storage == null)
                throw new ArgumentException("storage");

            m_identity = identity;
            m_storage = storage;
            m_logger = logger;


            if (!Guid.TryParse(identity.GetIdenfitier(), out m_streamID))
                throw new DomainException("Identity can not be converted to guid. Identity id: {0}", m_identity.GetIdenfitier());
        }

        #region Implementation of IRecordStorage

        public IEnumerable<SavedRecord> GetRecords(long afterVersion, int maxCount)
        {
            if (afterVersion < 0)
                throw new ArgumentOutOfRangeException("afterVersion", "Must be zero or greater.");

            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException("maxCount", "Must be more than zero.");

            if (afterVersion > long.MaxValue - maxCount)
                throw new ArgumentOutOfRangeException("maxCount", "Version will exceed long.MaxValue.");

            var records = new List<SavedRecord>();

            using (var stream = m_storage.OpenStream(m_streamID, (int)afterVersion, maxCount))
            {
                records.AddRange(stream.CommittedEvents.Select(committedEvent => (SavedRecord)committedEvent.Body));
            }

            return records;
        }

        public bool TryAppend(object[] content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            try
            {
                using (var stream = m_storage.OpenStream(m_streamID, 0, int.MaxValue))
                {
                    var versionInStore = stream.StreamRevision;

                    content.ToList().ForEach(m =>
                    {
                        var version = ++versionInStore;
                        var key = string.Format("{0}-{1:00000000}", m.GetType().Name, version);

                        var savedMessage = new SavedRecord(key, version, m);

                        stream.Add(new EventMessage { Body = savedMessage });
                    });

                    stream.CommitChanges(Guid.NewGuid());
                }

                return true;
            }
            catch (Exception e)
            {
                m_logger.LogError(e);

                return false;
            }
        }

        #endregion
    }
}
