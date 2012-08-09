using System;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class CommandHandlerTests
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

            var handler = new CommandHandler();
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

            var bus = new MemoryBusWithRouter(m_router);


            //
            // Queue Writer

            var queueWriter = new QueueWriterToBus(bus);


            //
            // Misc

            m_sender = new MessageSender(new IQueueWriter[] { queueWriter });
            m_id = new AccountID(Guid.NewGuid());
        }

        [Test]
        public void command_handler_should_handle_command_via_router()
        {
            // arrange
            var cmd = new CreateAccount(m_id);

            // act
            m_sender.SendOne(cmd);

            // assert
        }

        [Test]
        public void command_handler_should_handle_multiple_commands_via_router()
        {
            // arrange
            var c1 = new CreateAccount(m_id);
            var c2 = new CreateAccount(m_id);
            var c3 = new CreateAccount(m_id);

            // act
            m_sender.SendBatch(new object[] { c1, c2, c3});

            // assert
        }

        [Test]
        public void command_handler_should_handle_command()
        {
            // arrange
            var cmd = new CreateAccount(m_id);

            var handler = new CommandHandler();
            handler.WireToLambda<CreateAccount>(new CreateAccountHandler().Handle);

            // act
            handler.Handle(cmd);

            // assert
        }
    }
}