using System;
using System.Linq;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;
using Should;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class AggregateFactoryTests
    {
        [Test]
        public void aggregate_facotry_should_execute_store_and_dispatch()
        {
            // arrange
            var router = new MemoryMessageRouter();
            router.RegisterHandler<CreateAccount>(new CreateAccountHandler().Handle);

            var id = new AccountID(Guid.NewGuid());
            var storageFactory = new MemoryRecordStorageFactory();

            var bus = new MemoryBusWithRouter(router);
            var factory = new AccountFactory(bus, storageFactory);

            // act
            factory.ExecuteStoreDispatch(new CreateAccount(id));

            // assert
            var storage = storageFactory.GetOrCreateStorage(id);
            storage.GetRecords(0, int.MaxValue).Count().ShouldEqual(1);
        }
    }
}