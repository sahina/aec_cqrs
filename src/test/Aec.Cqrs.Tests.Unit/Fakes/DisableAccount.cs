namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class DisableAccount : ICommand<AccountID>
    {
        public DisableAccount(AccountID id)
        {
            Identity = id;
        }

        #region Implementation of ICommand<out AccountID>

        public AccountID Identity { get; private set; }

        #endregion
    }
}