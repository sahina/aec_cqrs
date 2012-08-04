using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Projections
{
    [DataContract]
    public sealed class LoginView
    {
        public string Display { get; set; }
        public SecurityID Security { get; set; }
        public string Token { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Key { get; set; }
        public DateTime LockedOutTillUtc { get; set; }
        public string LockoutMessage { get; set; }
        public LoginViewType Type { get; set; }
        public TimeSpan LoginTrackingThreshold { get; set; }
        public DateTime LastLoginUtc { get; set; }
        public IList<string> Permissions { get; set; }

        public LoginView()
        {
            Permissions = new List<string>(0);
        }
    }

    public enum LoginViewType
    {
        Undefined,
        Key,
        Password,
        Identity
    }
}
