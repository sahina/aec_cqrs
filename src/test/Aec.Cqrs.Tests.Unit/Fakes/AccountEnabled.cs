using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    [Serializable]
    public class AccountEnabled : IEvent<AccountID>
    {
        public AccountEnabled(AccountID id)
        {
            Identity = id;
        }

        #region Implementation of IEvent<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}