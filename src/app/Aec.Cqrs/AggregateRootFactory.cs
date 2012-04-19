using System.Collections.Generic;
using System.Linq;

namespace Aec.Cqrs
{
    public abstract class AggregateRootFactory
    {
        protected IEventBus EventBus { get; private set; }
        protected IRecordStorageFactory StorageFactory { get; private set; }

        protected AggregateRootFactory(IEventBus eventBus, IRecordStorageFactory storageFactory)
        {
            EventBus = eventBus;
            StorageFactory = storageFactory;
        }

        #region Factory

        public abstract Applied Execute(params ICommand<IIdentity>[] commands);

        public void ExecuteStoreDispatch(params ICommand<IIdentity>[] commands)
        {
            var applied = Execute(commands);

            // save applied events to storage
            AppendToStorage(applied.Events.First().Identity, applied);

            // dispatch applied events to message bus
            DispatchEvents(applied);
        }

        public void AppendToStorage(IIdentity id, Applied applied)
        {
            if (applied.Events == null)
                return;

            var storage = StorageFactory.GetOrCreateStorage(id);
            storage.TryAppend(applied.Events.Cast<IMessage>().ToArray());
        }

        public void DispatchEvents(Applied applied)
        {
            EventBus.PublishAll(applied.Events);
        }

        #endregion

        #region Private Methods

        protected static void ExecuteSafely<TIdentity>(IAggregate<TIdentity> aggregate, IEnumerable<ICommand<IIdentity>> commands)
            where TIdentity : IIdentity
        {
            foreach (var hubCommand in commands)
            {
                aggregate.Execute((ICommand<TIdentity>)hubCommand);
            }
        }

        #endregion
    }
}