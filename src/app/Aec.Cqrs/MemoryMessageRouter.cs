using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryMessageRouter : IRouteMessages, IRegisterMessageRoutes
    {
        private readonly IDictionary<Type, ICollection<Action<object>>> m_routes;

        public MemoryMessageRouter()
        {
            m_routes = new Dictionary<Type, ICollection<Action<object>>>();
        }

        #region Implementation of IRouteMessages

        public void Route(IMessage message)
        {
            ICollection<Action<object>> routes;

            if (!m_routes.TryGetValue(message.GetType(), out routes))
                routes = new List<Action<object>>();

            foreach (var route in routes)
            {
                if (message is ICommand<IIdentity>)
                    SystemObserver.Notify(new CommandHandled(message));
                if (message is IEvent<IIdentity>)
                    SystemObserver.Notify(new EventHandled(message));

                route(message);
            }
        }

        #endregion

        #region Implementation of IRegisterMessageRoutes

        public void RegisterHandler<T>(Action<T> handler) where T : class
        {
            var routingKey = typeof(T);
            ICollection<Action<object>> routes;

            if (!m_routes.TryGetValue(routingKey, out routes))
                m_routes[routingKey] = routes = new LinkedList<Action<object>>();

            routes.Add(message => handler((T)message));
        }

        #endregion
    }
}