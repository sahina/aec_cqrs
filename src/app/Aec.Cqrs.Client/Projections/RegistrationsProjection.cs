using System;
using Aec.Cqrs.Client.Events;

namespace Aec.Cqrs.Client.Projections
{
    public sealed class RegistrationsProjection
    {
        private readonly IDocumentWriter<RegistrationID, RegistrationView> m_writer;

        public RegistrationsProjection(IDocumentWriter<RegistrationID, RegistrationView> writer)
        {
            m_writer = writer;
        }

        public void When(RegistrationCreated e)
        {
            m_writer.Add(e.Identity, new RegistrationView
            {
                Status = "Processing registration",
                Registration = e.Identity
            });
        }

        public void When(RegistrationSucceeded e)
        {
            m_writer.UpdateOrThrow(e.Identity, v =>
            {
                v.HasProblems = false;
                v.Completed = true;
                v.Status = "Registration completed";
                v.UserID = e.UserID;
                v.UserDisplayName = e.UserDisplayName;
                v.UserToken = e.UserToken;
                v.SecurityID = e.SecurityID;
            });
        }

        public void When(RegistrationFailed e)
        {
            m_writer.Add(e.Identity, new RegistrationView
            {
                HasProblems = true,
                Status = "Problem discovered",
                Completed = true,
                Problem = string.Join(Environment.NewLine, e.Problems)
            });
        }
    }
}