namespace Aec.Cqrs.Client
{
    public sealed class AuthInfo
    {
        public readonly UserID Login;
        public readonly string Token;

        public AuthInfo(UserID login, string token)
        {
            Login = login;
            Token = token;
        }

        public static Maybe<AuthInfo> Parse(string cookieString)
        {
            if (string.IsNullOrEmpty(cookieString))
                return Maybe<AuthInfo>.Empty;
            if (!cookieString.StartsWith("v1|"))
                return Maybe<AuthInfo>.Empty;

            var strings = cookieString.Split('|');
            var login = strings[1];
            var token = strings[2];
            return new AuthInfo(new UserID(login), token);
        }

        public string ToCookieString()
        {
            return string.Format("v1|{0}|{1}", Login.GetIdenfitier(), Token);
        }
    }
}