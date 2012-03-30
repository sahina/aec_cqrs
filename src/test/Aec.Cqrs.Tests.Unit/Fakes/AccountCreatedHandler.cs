using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountCreatedHandler : IEventHandler<AccountCreated>
    {
        #region Implementation of IEventHandler<in AccountCreated>

        public void Handle(AccountCreated theEvent)
        {
            Console.WriteLine("-------> AccountCreatedHandler.Handle invoked");
        }

        #endregion
    }
}