using System.Collections.Generic;
using System.Linq;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountState : IAccountState, IAggregateState
    {
        public AccountID ID { get; private set; }
        public long Version { get; private set; }

        public AccountState(IEnumerable<IEvent<IIdentity>> eventHistory)
        {
            eventHistory.ToList().ForEach(Apply);
        }

        #region Implementation of IAccountState

        public void When(AccountCreated e)
        {
            ID = e.Identity;
        }

        #endregion

        #region Implementation of IAggregateState

        public void Apply(IEvent<IIdentity> e)
        {
            Version += 1;
            ((dynamic)this).When((dynamic)e);
        }

        #endregion
    }
}