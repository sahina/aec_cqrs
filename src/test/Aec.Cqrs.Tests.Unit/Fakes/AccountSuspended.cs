using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    [Serializable]
    public class AccountSuspended : IEvent<AccountID>
    {
        public AccountSuspended(AccountID id)
        {
            Identity = id;
        }

        #region Implementation of IEvent<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}