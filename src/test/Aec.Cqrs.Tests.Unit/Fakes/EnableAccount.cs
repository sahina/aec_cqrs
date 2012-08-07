namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class EnableAccount : ICommand<AccountID>
    {
        public EnableAccount(AccountID id)
        {
            Identity = id;
        }

        #region Implementation of ICommand<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}