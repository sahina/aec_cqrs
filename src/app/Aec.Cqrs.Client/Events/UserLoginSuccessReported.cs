using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class UserLoginSuccessReported : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public DateTime TimeUtc { get; private set; }

        [DataMember(Order = 3)]
        public string Ip { get; private set; }

        public UserLoginSuccessReported(SecurityID securityID, UserID userID, DateTime timeUtc, string ip)
            : base(securityID)
        {
            UserID = userID;
            TimeUtc = timeUtc;
            Ip = ip;
        }
    }
}