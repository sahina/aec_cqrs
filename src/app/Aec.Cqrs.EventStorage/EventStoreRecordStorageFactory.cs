using Aec.Infrastructure.Framework.Logging;
using EventStore;

namespace Aec.Cqrs.EventStorage
{
    public class EventStoreRecordStorageFactory : IRecordStorageFactory
    {
        private readonly IRecordStorageConfig m_config;
        private readonly ILogger m_logger;

        public EventStoreRecordStorageFactory(IRecordStorageConfig config, ILogger logger)
        {
            m_config = config;
            m_logger = logger;
        }

        public IRecordStorage GetOrCreateStorage(IIdentity id)
        {
            var eventStoreJo = (IStoreEvents) m_config.BuildConfiguration();

            return new EventStoreRecordStorage(id, eventStoreJo, m_logger);
        }
    }
}