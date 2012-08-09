using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryBusWithRouterAndHandler : ICommandBus, IEventBus
    {
        private readonly MessageHandler m_handler;

        public MemoryBusWithRouterAndHandler(MessageHandler handler)
        {
            m_handler = handler;
        }

        #region Implementation of ICommandBus

        public void Send(ImmutableEnvelope envelope)
        {
            m_handler.HandleEnvelope(envelope);
        }

        public void Send<T>(T command) where T : class, ICommand<IIdentity>
        {
            m_handler.Handle(command);
        }

        public void SendAll<T>(params T[] commands) where T : class, ICommand<IIdentity>
        {
            foreach (var command in commands)
                Send(command);
        }

        #endregion

        #region Implementation of IEventBus

        public void Publish<T>(T @event) where T : class, IEvent<IIdentity>
        {
            m_handler.Handle(@event);
        }

        public void PublishAll<T>(IEnumerable<T> events) where T : class, IEvent<IIdentity>
        {
            foreach (var @event in events)
                Publish(@event);
        }

        #endregion
    }
}