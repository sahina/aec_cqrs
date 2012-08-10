using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aec.Cqrs.WebUI.Infrastructure
{
    public sealed class WebEndpoint
    {
        private readonly IQueueWriter m_queueWriter;
        private readonly DocumentStorage m_store;

        public WebEndpoint(IQueueWriter queueWriter, DocumentStorage store)
        {
            m_queueWriter = queueWriter;
            m_store = store;
        }

        #region Public Methods

        public void SendOne(ICommand<IIdentity> command, string optionalID = null)
        {
            SendMessage(command, optionalID);
        }

        public void PublishOne(IEvent<IIdentity> e, string optionalID = null)
        {
            SendMessage(e, optionalID);
        }

        public Maybe<TView> GetView<TView>(IIdentity key)
        {
            return m_store.GetEntity<TView>(key);
        }

        public IEnumerable<TView> GetAllViews<TView>()
        {
            IEnumerable<TView> views;

            m_store.TryGetAllEntities(out views);

            return views;
        }

        #endregion

        #region Private Methods

        private void SendMessage(object message, string optionalID = null)
        {
            var auth = FormsAuth.GetSessionIdentityFromRequest();
            var envelopeId = optionalID ?? Guid.NewGuid().ToString().ToLowerInvariant();
            var eb = new EnvelopeBuilder(envelopeId);

            if (auth.HasValue)
            {
                eb.AddString("web-user", auth.Value.User.Identifier.ToString(CultureInfo.InvariantCulture));
                eb.AddString("web-token", auth.Value.Token);
            }
            eb.AddItem(message);

            m_queueWriter.PutMessage(eb.Build());
        }

        #endregion
    }
}
