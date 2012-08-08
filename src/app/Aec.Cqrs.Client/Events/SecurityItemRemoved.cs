using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class SecurityItemRemoved : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string Lookup { get; private set; }

        [DataMember(Order = 3)]
        public string Type { get; private set; }

        public SecurityItemRemoved(SecurityID securityID, UserID userId, string lookup, string type)
            : base(securityID)
        {
            UserID = userId;
            Lookup = lookup;
            Type = type;
        }
    }
}