using System;

namespace Aec.Cqrs
{
    public class DomainException : Exception
    {
        public DomainException() {}
        public DomainException(string message) : base(message) {}
        public DomainException(string format, params object[] args) : base(string.Format(format, args)) { }
        public DomainException(string message, Exception inner) : base(message, inner) { }
    }
}
