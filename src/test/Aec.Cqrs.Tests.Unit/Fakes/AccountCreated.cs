namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountCreated : IEvent<AccountID>
    {
        public AccountCreated(AccountID id)
        {
            Identity = id;
        }

        #region Implementation of IEvent<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}