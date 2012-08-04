using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class SecurityEvent : IEvent<SecurityID>
    {
        public SecurityEvent(SecurityID identity)
        {
            Identity = identity;
        }

        #region Implementation of IEvent<out SecurityID>

        public SecurityID Identity { get; private set; }

        #endregion
    }
}