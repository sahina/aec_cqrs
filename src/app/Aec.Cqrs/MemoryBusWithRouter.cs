using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryBusWithRouter : IEventBus, ICommandBus
    {
        private readonly IRouteMessages m_router;

        public MemoryBusWithRouter(IRouteMessages router)
        {
            m_router = router;
        }

        #region Implementation of IEventBus

        public void Publish<T>(T @event) where T : class, IEvent<IIdentity>
        {
            m_router.Route(@event);
        }

        public void PublishAll<T>(IEnumerable<T> events) where T : class, IEvent<IIdentity>
        {
            foreach (var e in events)
                Publish(e);
        }

        #endregion

        #region Implementation of ICommandBus

        public void Send(ImmutableEnvelope envelope)
        {
            foreach (var immutableMessage in envelope.Items)
            {
                var cmd = immutableMessage.Content as ICommand<IIdentity>;
                if (cmd != null)
                    Send(cmd);

                var evnt = immutableMessage.Content as IEvent<IIdentity>;
                if (evnt != null)
                    Publish(evnt);
            }
        }

        public void Send<T>(T command) where T : class, ICommand<IIdentity>
        {
            m_router.Route(command);
        }

        public void SendAll<T>(params T[] commands) where T : class, ICommand<IIdentity>
        {
            foreach (var command in commands)
                Send(command);
        }

        #endregion
    }
}
