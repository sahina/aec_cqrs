using System.Collections.Generic;

namespace Aec.Cqrs
{
    /// <summary>
    /// Is published whenever an event is sent.
    /// </summary>
    public sealed class EnvelopeSent : ISystemEvent
    {
        public readonly string QueueName;
        public readonly string EnvelopeId;
        public readonly bool Transactional;
        public readonly string[] MappedTypes;
        public readonly ICollection<ImmutableAttribute> Attributes;

        public EnvelopeSent(string queueName, string envelopeId, bool transactional,
                            string[] mappedTypes, ICollection<ImmutableAttribute> attributes)
        {
            QueueName = queueName;
            EnvelopeId = envelopeId;
            Transactional = transactional;
            MappedTypes = mappedTypes;
            Attributes = attributes;
        }

        public override string ToString()
        {
            return string.Format("Envelope Sent: {0}{1} to '{2}' as [{3}]",
                                 string.Join("+", MappedTypes),
                                 Transactional ? " +tx" : "",
                                 QueueName,
                                 EnvelopeId);
        }
    }
}