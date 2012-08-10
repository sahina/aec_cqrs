using System.Diagnostics;
using System.Web;
using Aec.Cqrs.Client;

namespace Aec.Cqrs.WebUI.Infrastructure
{
    /// <summary>
    /// Static class that provides strongly-typed access to the session (user)-
    /// specific objects and variables. This includes session-specific container
    /// </summary>
    public static class GlobalState
    {
        private const string ACCOUNT_SESSION_KEY = "GlobalSetup_ASK";

        public static void Clear()
        {
            var session = HttpContext.Current.Session;
            session.Clear();
        }

        /// <summary>
        /// Single point of entry to initialize the session
        /// </summary>
        /// <param name="accountSession">The account session.</param>
        public static void InitializeSession(SessionIdentity accountSession)
        {
            var session = HttpContext.Current.Session;
            session[ACCOUNT_SESSION_KEY] = accountSession;
        }

        /// <summary>
        /// Initializes the session, using the auth information
        /// associated with the current request
        /// </summary>
        public static void InitializeSessionFromRequest()
        {
            var context = HttpContext.Current;
            // we are fine
            if (context.Session[ACCOUNT_SESSION_KEY] != null)
                return;

            // unauthenticated session here
            if (!context.Request.IsAuthenticated)
                return;

            // authenticated session but without our data.
            // recover expired session (or use cookie)

            Debug.WriteLine("Session initialization attempt");
            FormsAuth
                .GetSessionIdentityFromRequest()
                .Apply(InitializeSession);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAuthenticated
        {
            get { return HttpContext.Current.Session[ACCOUNT_SESSION_KEY] != null; }
        }

        /// <summary>
        /// Gets the account associated with the current session.
        /// </summary>
        /// <value>The account associated with the current session.</value>
        public static SessionIdentity Identity
        {
            get
            {
                // session recovery is handled by the global handler
                return (SessionIdentity)HttpContext.Current.Session[ACCOUNT_SESSION_KEY];
            }
        }

        public static UserID User
        {
            get { return Identity.User; }
        }

        public static SecurityID Security
        {
            get { return Identity.Security; }
        }
    }
}