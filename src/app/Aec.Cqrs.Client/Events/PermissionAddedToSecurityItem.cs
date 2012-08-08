using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class PermissionAddedToSecurityItem : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string DisplayName { get; private set; }

        [DataMember(Order = 3)]
        public string Permission { get; private set; }

        [DataMember(Order = 4)]
        public string Token { get; private set; }

        public PermissionAddedToSecurityItem(SecurityID securityID, UserID userId, string displayName, string permission, string token)
            : base(securityID)
        {
            UserID = userId;
            DisplayName = displayName;
            Permission = permission;
            Token = token;
        }
    }
}