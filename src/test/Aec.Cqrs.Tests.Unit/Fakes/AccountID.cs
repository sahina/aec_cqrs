using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountID : Identity<Guid>
    {
        public AccountID(Guid id)
        {
            ID = id;
        }
    }
}