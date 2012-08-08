using System.Runtime.Serialization;

namespace Aec.Cqrs.Client
{
    [DataContract]
    public class SecurityInfo
    {
        [DataMember(Order = 1)]
        public SecurityID SecurityID { get; private set; }
        
        [DataMember(Order = 2)]
        public string Login { get; private set; }
        
        [DataMember(Order = 3)]
        public string Pwd { get; private set; }
        
        [DataMember(Order = 4)]
        public string UserDisplay { get; private set; }
        
        [DataMember(Order = 5)]
        public string OptionalIdentity { get; private set; }

        public SecurityInfo(SecurityID securityId, string login, string pwd, string userDisplay, string optionalIdentity)
        {
            SecurityID = securityId;
            Login = login;
            Pwd = pwd;
            UserDisplay = userDisplay;
            OptionalIdentity = optionalIdentity;
        }
    }
}