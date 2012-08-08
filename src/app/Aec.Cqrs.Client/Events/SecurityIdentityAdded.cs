using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class SecurityIdentityAdded : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }
        [DataMember(Order = 2)]
        public string DisplayName { get; private set; }
        [DataMember(Order = 3)]
        public string Token { get; private set; }

        public SecurityIdentityAdded(SecurityID id, UserID userID, string displayName, string token)
            : base(id)
        {
            UserID = userID;
            DisplayName = displayName;
            Token = token;
        }
    }
}
