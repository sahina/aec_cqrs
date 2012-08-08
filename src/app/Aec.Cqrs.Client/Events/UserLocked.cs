using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class UserLocked : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string LockReason { get; private set; }

        [DataMember(Order = 3)]
        public DateTime LockedTillUtc { get; private set; }

        public UserLocked(SecurityID securityID, UserID userID, string lockReason, DateTime lockedTillUtc)
            : base(securityID)
        {
            UserID = userID;
            LockReason = lockReason;
            LockedTillUtc = lockedTillUtc;
        }
    }
}