using System;
using System.Collections.Generic;
using System.Linq;

namespace Aec.Cqrs
{
    /// <summary>
    /// Deserialized message representation
    /// </summary>
    public class ImmutableEnvelope
    {
        private readonly ImmutableAttribute[] m_attributes;

        public readonly string EnvelopeId;
        public readonly DateTime DeliverOnUtc;
        public readonly DateTime CreatedOnUtc;
        public readonly ImmutableMessage[] Items;

        public ImmutableEnvelope(string envelopeId, ImmutableAttribute[] attributes, ImmutableMessage[] items,
                                 DateTime deliverOnUtc, DateTime createdOnUtc)
        {
            EnvelopeId = envelopeId;
            DeliverOnUtc = deliverOnUtc;
            m_attributes = attributes;
            Items = items;
            CreatedOnUtc = createdOnUtc;
        }

        public string GetAttribute(string name)
        {
            return m_attributes.First(n => n.Key == name).Value;
        }

        public string GetAttribute(string name, string defaultValue)
        {
            foreach (var attribute in m_attributes)
            {
                if (attribute.Key == name)
                    return attribute.Value;
            }
            return defaultValue;
        }

        public ICollection<ImmutableAttribute> GetAllAttributes()
        {
            return m_attributes;
        }

        public override string ToString()
        {
            //return string.Format("{0} - CreatedOnUtc: {1}, DeliverOnUtc: {2}, [{3}]",
            return string.Format("EnvelopeID: {0} - {1}",
                EnvelopeId,
                //CreatedOnUtc,
                //DeliverOnUtc,
                String.Join<ImmutableMessage>("+", Items)
                );
        }
    }
}