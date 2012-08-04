using System;
using System.Collections.Generic;

namespace Aec.Cqrs.Client
{
    public sealed class SessionIdentity
    {
        public readonly UserID User;
        public readonly SecurityID Security;

        public readonly string UserName;
        public readonly string SessionDisplay;
        public readonly string CookieString;
        public readonly HashSet<string> Permissions;
        public readonly string Token;

        public static SessionIdentity Create(string dispay, UserID user, string token,
                                             SecurityID sec, params string[] permissions)
        {
            var auth = new AuthInfo(user, token);
            return new SessionIdentity(user, sec, dispay, auth.ToCookieString(), permissions, token);
        }

        public SessionIdentity(UserID user, SecurityID security, string userName,
                               string cookieString, IEnumerable<string> permissions, string token)
        {
            User = user;
            Security = security;
            UserName = userName;
            SessionDisplay = String.Format("{0} ({1})", UserName, User.GetIdenfitier());

            CookieString = cookieString;
            Permissions = new HashSet<string>(permissions);
            Token = token;
        }
    }
}