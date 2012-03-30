using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount>
    {
        #region Implementation of ICommandHandler<in CreateAccount>

        public void Handle(CreateAccount command)
        {
            Console.WriteLine("-------> CreateAccountHandler.Handle invoked");
        }

        #endregion
    }
}