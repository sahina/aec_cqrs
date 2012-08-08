using System;
using System.Globalization;

namespace Aec.Cqrs.Client.Web
{
    public sealed class WebEndpoint
    {
        private readonly IQueueWriter m_queueWriter;

        public WebEndpoint(IQueueWriter queueWriter)
        {
            m_queueWriter = queueWriter;
        }

        public void SendOne(ICommand<IIdentity> command, string optionalID = null)
        {
            SendMessage(command, optionalID);
        }

        private void SendMessage(object command, string optionalID = null)
        {
            var auth = FormsAuth.GetSessionIdentityFromRequest();
            var envelopeId = optionalID ?? Guid.NewGuid().ToString().ToLowerInvariant();
            var eb = new EnvelopeBuilder(envelopeId);

            if (auth.HasValue)
            {
                eb.AddString("web-user", auth.Value.User.Identifier.ToString(CultureInfo.InvariantCulture));
                eb.AddString("web-token", auth.Value.Token);
            }
            eb.AddItem(command);

            m_queueWriter.PutMessage(eb.Build());
        }
    }
}
