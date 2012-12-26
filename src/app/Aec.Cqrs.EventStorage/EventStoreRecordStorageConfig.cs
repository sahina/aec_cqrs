using System;
using Aec.Infrastructure.Framework.Logging;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Persistence.SqlPersistence.SqlDialects;

namespace Aec.Cqrs.EventStorage
{
    public class EventStoreRecordStorageConfig : IRecordStorageConfig
    {
        private readonly IEventBus m_eventBus;
        private readonly ILogger m_logger;
        private readonly string m_connectionName;

        public EventStoreRecordStorageConfig(IEventBus eventBus, ILogger logger, string connectionName)
        {
            m_eventBus = eventBus;
            m_logger = logger;
            m_connectionName = connectionName;
        }

        public object BuildConfiguration()
        {
            return Wireup.Init()
                         .LogToOutputWindow()
                         .UsingInMemoryPersistence()
                         .UsingSqlPersistence(m_connectionName) // Connection string is in app.config
                             .WithDialect(new MsSqlDialect())
                             .EnlistInAmbientTransaction() // two-phase commit
                             .InitializeStorageEngine()
                         .UsingJsonSerialization()
                            .Compress()
                         //.HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
                         .UsingSynchronousDispatchScheduler()
                            .DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
                        .Build();
        }

        private void DispatchCommit(Commit commit)
        {
            try
            {
                foreach (var @event in commit.Events)
                    m_eventBus.Publish((IEvent<IIdentity>) ((SavedRecord)@event.Body).Content);

            }
            catch (Exception e)
            {
                m_logger.LogError(e);
            }
        }
    }
}