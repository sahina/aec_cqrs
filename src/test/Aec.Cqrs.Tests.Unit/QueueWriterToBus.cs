namespace Aec.Cqrs.Tests.Unit
{
    public class QueueWriterToBus : IQueueWriter
    {
        private readonly ICommandBus m_bus;

        public QueueWriterToBus(ICommandBus bus)
        {
            m_bus = bus;
        }

        #region Implementation of IQueueWriter

        public string Name
        {
            get { return "QueueWriterToBus"; }
        }

        public void PutMessage(ImmutableEnvelope envelope)
        {
            SystemObserver.Notify(new EnvelopeDispatched(envelope, Name));

            m_bus.Send(envelope);
        }

        #endregion
    }
}