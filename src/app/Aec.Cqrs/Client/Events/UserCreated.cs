using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class UserCreated : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public TimeSpan ActivityThreshold { get; private set; }

        public UserCreated(SecurityID securityID, UserID userID, TimeSpan activityThreshold)
            : base(securityID)
        {
            UserID = userID;
            ActivityThreshold = activityThreshold;
        }
    }
}