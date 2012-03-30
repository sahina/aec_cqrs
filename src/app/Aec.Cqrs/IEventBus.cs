using System.Collections.Generic;

namespace Aec.Cqrs
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : class, IEvent<IIdentity>;
        void PublishAll<T>(IEnumerable<T> events) where T : class, IEvent<IIdentity>;
    }
}