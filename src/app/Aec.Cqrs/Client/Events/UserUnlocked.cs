using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class UserUnlocked : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string UnlockReason { get; private set; }

        public UserUnlocked(SecurityID securityID, UserID userID, string unlockReason)
            : base(securityID)
        {
            UserID = userID;
            UnlockReason = unlockReason;
        }
    }
}