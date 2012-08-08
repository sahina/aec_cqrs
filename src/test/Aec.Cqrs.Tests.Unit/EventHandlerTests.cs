using System;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class EventHandlerTests
    {
        private MessageSender m_sender;
        private AccountID m_id;
        private IRegisterMessageRoutes m_registerRoutes;
        private IRouteMessages m_router;

        [SetUp]
        public void Setup()
        {
            //
            // observers

            SystemObserver.Setup(new IObserver<ISystemEvent>[] { new ConsoleObserver() });


            //
            // message handlers

            var handler = new MessageHandler();
            handler.WireToLambda<CreateAccount>(new CreateAccountHandler().Handle);
            handler.WireToLambda<AccountCreated>(new AccountCreatedHandler().Handle);


            //
            // message router

            var router = new MemoryMessageRouter();

            m_router = router;
            m_registerRoutes = router;

            m_registerRoutes.RegisterHandler<CreateAccount>(handler.Handle);
            m_registerRoutes.RegisterHandler<AccountCreated>(handler.Handle);


            //
            // message bus

            var bus = new MemoryBus(m_router);


            //
            // Queue Writer

            var queueWriter = new QueueWriterToBus(bus);


            //
            // Misc

            m_sender = new MessageSender(new IQueueWriter[] { queueWriter });
            m_id = new AccountID(Guid.NewGuid());
        }

        [Test]
        public void event_handler_should_handle_event_via_router()
        {
            // arrange
            var e = new AccountCreated(m_id);

            // act
            m_sender.SendOne(e);

            // assert
        }

        [Test]
        public void event_handler_should_handle_multiple_events_via_router()
        {
            // arrange
            var e1 = new AccountCreated(m_id);
            var e2 = new AccountCreated(m_id);
            var e3 = new AccountCreated(m_id);

            // act
            m_sender.SendBatch(new object[] { e1, e2, e3 });

            // assert
        }

        [Test]
        public void event_handler_should_handle_event()
        {
            // arrange
            var e = new AccountCreated(m_id);

            var handler = new MessageHandler();
            handler.WireToLambda<AccountCreated>(new AccountCreatedHandler().Handle);

            // act
            handler.Handle(e);

            // assert
        }
    }
}