using System;

namespace Aec.Cqrs.Tests.Unit.Fakes
{
    public class ExpectedTestException : Exception
    {
        public ExpectedTestException(string message)
            : base(message)
        {
        }
    }
}
