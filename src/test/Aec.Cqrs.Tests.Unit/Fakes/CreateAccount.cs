namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class CreateAccount : ICommand<AccountID>
    {
        public CreateAccount(AccountID id)
        {
            ID = id;
        }
        #region Implementation of ICommand<out AccountID>

        public AccountID ID { get; private set; }

        #endregion
    }
}
