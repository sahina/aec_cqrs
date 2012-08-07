namespace Aec.Cqrs.Tests.Unit
{
    public class QueueWriterToBus : IQueueWriter
    {
        private readonly ICommandBus m_bus;
        private readonly IEnvelopeSerializer m_envelopeSerializer;

        public QueueWriterToBus(ICommandBus bus, IEnvelopeSerializer envelopeSerializer)
        {
            m_bus = bus;
            m_envelopeSerializer = envelopeSerializer;
        }

        #region Implementation of IQueueWriter

        public string Name
        {
            get { return "Bus;"; }
        }

        public void PutMessage(byte[] envelope)
        {
            //SystemObserver.Notify(new EnvelopeDispatched(envelope, Name));

            //m_bus.Send(envelope);
        }

        public void PutMessage(ImmutableEnvelope envelope)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}