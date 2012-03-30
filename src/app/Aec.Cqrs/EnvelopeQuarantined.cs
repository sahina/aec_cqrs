using System;

namespace Aec.Cqrs
{
    /// <summary>
    /// Is publsihed when an evelope is quarantined
    /// </summary>
    public sealed class EnvelopeQuarantined : ISystemEvent
    {
        public Exception LastException { get; private set; }
        public string Dispatcher { get; private set; }
        public ImmutableEnvelope Envelope { get; private set; }

        public EnvelopeQuarantined(Exception lastException, string dispatcher, ImmutableEnvelope envelope)
        {
            LastException = lastException;
            Dispatcher = dispatcher;
            Envelope = envelope;
        }

        public override string ToString()
        {
            return string.Format("Envelope Quarantined: '{0}': {1}", Envelope.EnvelopeId, LastException.Message);
        }
    }
}