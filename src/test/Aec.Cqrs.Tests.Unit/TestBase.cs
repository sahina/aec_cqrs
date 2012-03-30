using System;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Aec.Cqrs.Tests.Unit
{
    public class TestBase
    {
        protected MessageSender Sender;
        protected AccountID ID;
        protected ICommandBus CommandBus;
        protected IEventBus EventBus;
        protected IRegisterMessageRoutes Router;

        [SetUp]
        public void Setup()
        {
            //
            // observers

            SystemObserver.Setup(new IObserver<ISystemEvent>[] { new ConsoleObserver() });


            //
            // message router

            Router = new MemoryMessageRouter();

            Router.RegisterHandler<CreateAccount>(new CreateAccountHandler().Handle);
            Router.RegisterHandler<AccountCreated>(new AccountCreatedHandler().Handle);


            //
            // message bus

            var bus = new NullBus();
            CommandBus = bus;
            EventBus = bus;


            //
            // Queue Writer

            var queueWriter = new DefaultQueueWriter(bus);


            //
            // Misc

            Sender = new MessageSender(new IQueueWriter[] { queueWriter });
            ID = new AccountID(Guid.NewGuid());
        }
    }
}