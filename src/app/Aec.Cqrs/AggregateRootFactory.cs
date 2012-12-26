using System;
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

        public virtual void AppendToStorage(IIdentity id, Applied applied)
        {
            if (applied.Events == null)
                return;

            var storage = StorageFactory.GetOrCreateStorage(id);
            storage.TryAppend(applied.Events.Cast<IMessage>().ToArray());
        }

        public virtual void DispatchEvents(Applied applied)
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

    //public class AggregateRootFactory2<TAggregate, TAggregateState>
    //    where TAggregate : IAggregate<IIdentity>
    //    where TAggregateState : IAggregateState
    //{
    //    public IEventBus EventBus { get; private set; }
    //    public IRecordStorageFactory StorageFactory { get; private set; }

    //    public AggregateRootFactory2(IEventBus eventBus, IRecordStorageFactory storageFactory)
    //    {
    //        EventBus = eventBus;
    //        StorageFactory = storageFactory;
    //    }

    //    #region Factory

    //    public Applied Execute(Func<TAggregate> aggregate, Func<TAggregateState> state, params ICommand<IIdentity>[] commands)
    //    {
    //        if (commands == null)
    //            throw new ArgumentNullException("commands");

    //        var incomingCommands = commands.ToList();
    //        var id = incomingCommands.First().Identity;

    //        var storage = StorageFactory.GetOrCreateStorage(id);
    //        var records = storage.GetRecords(0, int.MaxValue).ToList();
    //        var eventsInStore = records.Select(r => (IEvent<IIdentity>)r.Content).ToList();

    //        var applied = new Applied();

    //        if (eventsInStore.Any())
    //            applied.Version = records.Last().Version;

    //        var a = aggregate();

    //        //var state = new CenterPatientAggregateState(eventsInStore);
    //        //var aggregate = new CenterPatientAggregate(state, applied.Events.Add);

    //        ExecuteSafely(aggregate, incomingCommands);

    //        return applied;
    //    }

    //    public void ExecuteStoreDispatch(params ICommand<IIdentity>[] commands)
    //    {
    //        var applied = Execute(commands);

    //        // save applied events to storage
    //        AppendToStorage(applied.Events.First().Identity, applied);

    //        // dispatch applied events to message bus
    //        DispatchEvents(applied);
    //    }

    //    public virtual void AppendToStorage(IIdentity id, Applied applied)
    //    {
    //        if (applied.Events == null)
    //            return;

    //        var storage = StorageFactory.GetOrCreateStorage(id);
    //        storage.TryAppend(applied.Events.Cast<IMessage>().ToArray());
    //    }

    //    public virtual void DispatchEvents(Applied applied)
    //    {
    //        EventBus.PublishAll(applied.Events);
    //    }

    //    #endregion

    //    #region Private Methods

    //    protected static void ExecuteSafely<TIdentity>(IAggregate<TIdentity> aggregate, IEnumerable<ICommand<IIdentity>> commands)
    //        where TIdentity : IIdentity
    //    {
    //        foreach (var hubCommand in commands)
    //        {
    //            aggregate.Execute((ICommand<TIdentity>)hubCommand);
    //        }
    //    }

    //    #endregion
    //}
}