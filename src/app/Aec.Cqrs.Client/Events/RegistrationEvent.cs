using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class RegistrationEvent : IEvent<RegistrationID>
    {
        public RegistrationEvent(RegistrationID registrationIdentity)
        {
            Identity = registrationIdentity;
        }

        #region Implementation of IEvent<out RegistrationID>

        public RegistrationID Identity { get; private set; }

        #endregion
    }
}