using System;
using System.Security.Principal;

namespace Aec.Cqrs.Client
{
    /// <summary>
    /// Implementation of <see cref="System.Security.Principal.IPrincipal"/> that provides
    /// backward compatibility for legacy authorization rules and ELMAH.
    /// </summary>
    [Serializable]
    public sealed class AuthPrincipal : MarshalByRefObject, IPrincipal
    {
        private readonly System.Security.Principal.IIdentity m_identity;

        /// <summary>
        /// Account associated with the principal
        /// </summary>
        public readonly SessionIdentity Identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPrincipal"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public AuthPrincipal(SessionIdentity account)
        {
            Identity = account;
            m_identity = new GenericIdentity(account.SessionDisplay);
        }

        bool IPrincipal.IsInRole(string role)
        {
            return false;
        }

        System.Security.Principal.IIdentity IPrincipal.Identity
        {
            get { return m_identity; }
        }
    }
}