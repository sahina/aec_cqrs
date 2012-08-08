using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class RegistrationSucceeded : RegistrationEvent
    {
        [DataMember(Order = 1)]
        public SecurityID SecurityID { get; private set; }

        [DataMember(Order = 2)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 3)]
        public string UserDisplayName { get; private set; }

        [DataMember(Order = 4)]
        public string UserToken { get; private set; }

        public RegistrationSucceeded(RegistrationID registrationIdentity, SecurityID securityID, UserID userID,
                                     string userDisplayName, string userToken)
            : base(registrationIdentity)
        {
            SecurityID = securityID;
            UserID = userID;
            UserDisplayName = userDisplayName;
            UserToken = userToken;
        }
    }
}