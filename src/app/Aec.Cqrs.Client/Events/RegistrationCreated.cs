using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class RegistrationCreated : RegistrationEvent
    {
        [DataMember(Order = 1)]
        public DateTime RegisteredUtc { get; private set; }

        [DataMember(Order = 2)]
        public SecurityInfo Security { get; private set; }

        public RegistrationCreated(RegistrationID registrationIdentity, DateTime registeredUtc, SecurityInfo security)
            : base(registrationIdentity)
        {
            RegisteredUtc = registeredUtc;
            Security = security;
        }
    }
}