using System;
using System.Linq;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class Account : IAccount, IAggregate<AccountID>
    {
        private readonly AccountState m_state;
        private readonly Action<IEvent<AccountID>> m_observer;

        public Account(AccountState state, Action<IEvent<AccountID>> observer)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            if (observer == null)
                throw new ArgumentNullException("observer");

            m_state = state;
            m_observer = observer;
        }

        #region Protected Methods

        void Apply(IEvent<AccountID> e)
        {
            m_state.Apply(e);
            m_observer(e);
        }

        #endregion

        #region Implementation of IAccount

        public void When(CreateAccount c)
        {
            if (m_state.Version != 0)
                throw new DomainException("Account has non-zero version");

            Apply(new AccountCreated(c.Identity));
        }

        #endregion

        #region Implementation of IAggregate<in AccountID>

        public void Execute(ICommand<AccountID> c)
        {
            RedirectToWhen.InvokeCommand(this, c);
        }

        #endregion
    }
}