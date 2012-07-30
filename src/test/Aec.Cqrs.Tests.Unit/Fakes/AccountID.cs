using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    [Serializable]
    public class AccountID : Identity<Guid>
    {
        public AccountID(Guid id)
        {
            Identifier = id;
        }
    }
}