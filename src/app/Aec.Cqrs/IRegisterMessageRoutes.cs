using System;

namespace Aec.Cqrs
{
    public interface IRegisterMessageRoutes
    {
        void RegisterHandler<T>(Action<T> handler) where T : class;
    }
}