using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class SecurityItemDisplayNameUpdated : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string DisplayName { get; private set; }

        public SecurityItemDisplayNameUpdated(SecurityID securityID, UserID userId, string displayName)
            : base(securityID)
        {
            UserID = userId;
            DisplayName = displayName;
        }
    }
}