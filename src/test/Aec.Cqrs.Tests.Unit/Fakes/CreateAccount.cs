namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class CreateAccount : ICommand<AccountID>
    {
        public CreateAccount(AccountID id)
        {
            Identity = id;
        }
        #region Implementation of ICommand<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}
