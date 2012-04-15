using System;
using System.Linq;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountFactory : AggregateRootFactory, IAggregateRootFactory
    {
        public AccountFactory(IEventBus eventBus, IRecordStorageFactory storageFactory)
            : base(eventBus, storageFactory)
        {
        }

        #region Overrides of RepositoryBase

        public override Applied Execute(params ICommand<IIdentity>[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            var incomingCommands = commands.ToList();
            var id = incomingCommands.First().ID;

            var storage = StorageFactory.GetOrCreateStorage(id);
            var records = storage.GetRecords(0, int.MaxValue).ToList();
            var eventsInStore = records.Select(r => (IEvent<IIdentity>)r.Content).ToList();

            var applied = new Applied();

            if (eventsInStore.Any())
                applied.Version = records.Last().Version;

            var state = new AccountState(eventsInStore);
            var aggregate = new Account(state, applied.Events.Add);

            ExecuteSafely(aggregate, incomingCommands);

            return applied;
        }

        #endregion
    }
}