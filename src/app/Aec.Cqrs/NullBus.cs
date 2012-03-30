using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class NullBus : IEventBus, ICommandBus
    {
        #region Implementation of IEventBus

        public virtual void Publish<T>(T @event) where T : class, IEvent<IIdentity>
        {

        }

        public virtual void PublishAll<T>(IEnumerable<T> events) where T : class, IEvent<IIdentity>
        {

        }

        #endregion

        #region Implementation of ICommandBus

        public virtual void Send(ImmutableEnvelope envelope)
        {

        }

        public virtual void Send<T>(T command) where T : class, ICommand<IIdentity>
        {

        }

        public virtual void SendAll<T>(params T[] commands) where T : class, ICommand<IIdentity>
        {

        }

        #endregion
    }
}