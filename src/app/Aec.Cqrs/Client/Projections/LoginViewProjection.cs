using System;
using Aec.Cqrs.Client.Events;

namespace Aec.Cqrs.Client.Projections
{
    public sealed class LoginViewProjection
    {
        private static readonly TimeSpan s_defaultThreshold = TimeSpan.FromMinutes(5);
        private readonly IDocumentWriter<UserID, LoginView> m_writer;

        public LoginViewProjection(IDocumentWriter<UserID, LoginView> writer)
        {
            m_writer = writer;
        }

        public void When(SecurityPasswordAdded e)
        {
            m_writer.Add(e.UserID, new LoginView
            {
                Security = e.Identity,
                Display = e.DisplayName,
                Token = e.Token,
                PasswordHash = e.PasswordHash,
                PasswordSalt = e.PasswordSalt,
                Type = LoginViewType.Password,
                LoginTrackingThreshold = s_defaultThreshold
            });
        }

        public void When(SecurityIdentityAdded e)
        {
            m_writer.Add(e.UserID, new LoginView
            {
                Security = e.Identity,
                Display = e.DisplayName,
                Token = e.Token,
                Type = LoginViewType.Identity,
                LoginTrackingThreshold = s_defaultThreshold
            });
        }


        public void When(UserLocked e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv =>
            {
                lv.LockedOutTillUtc = e.LockedTillUtc;
                lv.LockoutMessage = e.LockReason;
            });
        }
        public void When(UserUnlocked e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv =>
            {
                lv.LockedOutTillUtc = DateTime.MinValue;
                lv.LockoutMessage = null;
            });
        }

        public void When(UserCreated e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv => { lv.LoginTrackingThreshold = e.ActivityThreshold; });
        }

        public void When(UserLoginSuccessReported e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv => { lv.LastLoginUtc = e.TimeUtc; });
        }
        public void When(SecurityItemDisplayNameUpdated e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv => lv.Display = e.DisplayName);
        }
        public void When(SecurityItemRemoved e)
        {
            m_writer.TryDelete(e.UserID);
        }

        public void When(PermissionAddedToSecurityItem e)
        {
            m_writer.UpdateOrThrow(e.UserID, lv => lv.Permissions.Add(e.Permission));
        }
    }
}
