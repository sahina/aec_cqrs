namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountCreated : IEvent<AccountID>
    {
        public AccountCreated(AccountID id)
        {
            ID = id;
        }

        #region Implementation of IEvent<out AccountID>

        public AccountID ID { get; private set; }

        #endregion
    }
}