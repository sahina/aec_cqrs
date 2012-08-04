using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class SecurityPasswordAdded : SecurityEvent
    {
        [DataMember(Order = 1)]
        public UserID UserID { get; private set; }

        [DataMember(Order = 2)]
        public string DisplayName { get; private set; }

        [DataMember(Order = 3)]
        public string Login { get; private set; }

        [DataMember(Order = 4)]
        public string PasswordHash { get; private set; }

        [DataMember(Order = 5)]
        public string PasswordSalt { get; private set; }

        [DataMember(Order = 6)]
        public string Token { get; private set; }

        public SecurityPasswordAdded(SecurityID identity, UserID userID, string displayName,
                                     string login, string passwordHash, string passwordSalt, string token)
            : base(identity)
        {
            UserID = userID;
            DisplayName = displayName;
            Login = login;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Token = token;
        }
    }
}