namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public interface IAccountState
    {
        void When(AccountCreated e);
    }
}