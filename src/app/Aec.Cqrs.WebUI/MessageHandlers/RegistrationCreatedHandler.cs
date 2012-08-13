using Aec.Cqrs.Client;
using Aec.Cqrs.Client.Events;
using Aec.Cqrs.Client.Projections;

namespace Aec.Cqrs.WebUI.MessageHandlers
{
    public class RegistrationCreatedHandler : IEventHandler<RegistrationCreated>
    {
        private readonly IDocumentWriter<RegistrationID, RegistrationView> m_writer;

        public RegistrationCreatedHandler(IDocumentWriter<RegistrationID, RegistrationView> writer)
        {
            m_writer = writer;
        }

        #region Implementation of IEventHandler<in RegistrationCreated>

        public void Handle(RegistrationCreated theEvent)
        {
            var projections = new RegistrationsProjection(m_writer);

            projections.When(theEvent);
        }

        #endregion
    }
}