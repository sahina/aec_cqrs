using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class AccountID : AbstractIdentity<Guid>
    {
        public AccountID(Guid id)
        {
            ID = id;
        }
    }
}