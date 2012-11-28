using System;

namespace Aec.Cqrs
{
    public abstract class AggregateState : IAggregateState
    {
        public abstract Action<int> IncrementVersion();
 
        #region Implementation of IAggregateState

        public void Mutate(IEvent<IIdentity> e)
        {
            IncrementVersion();
            ((dynamic) this).When((dynamic) e);
        }

        #endregion
    }
}