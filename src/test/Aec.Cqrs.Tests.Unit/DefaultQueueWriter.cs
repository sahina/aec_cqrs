namespace Aec.Cqrs.Tests.Unit
{
    public class DefaultQueueWriter : IQueueWriter
    {
        private readonly ICommandBus m_bus;

        public DefaultQueueWriter(ICommandBus bus)
        {
            m_bus = bus;
        }

        #region Implementation of IQueueWriter

        public string Name
        {
            get { return "Unit Test Envelope Queue"; }
        }

        public void PutMessage(ImmutableEnvelope envelope)
        {
            SystemObserver.Notify(new EnvelopeDispatched(envelope, Name));

            m_bus.Send(envelope);
        }

        #endregion
    }
}
