using System;
using System.Collections.Generic;
using System.Linq;

namespace Aec.Cqrs
{
    public sealed class EnvelopeBuilder
    {
        private readonly IDictionary<string, string> m_attributes = new Dictionary<string, string>();
        private DateTime m_createdOnUtc;
        private DateTime m_deliverOnUtc;

        public readonly string EnvelopeId;
        public readonly IList<MessageBuilder> Items = new List<MessageBuilder>();

        public EnvelopeBuilder(string envelopeId)
        {
            m_createdOnUtc = DateTime.UtcNow;
            m_deliverOnUtc = m_createdOnUtc;

            EnvelopeId = envelopeId;
        }

        public void AddString(string key, string value)
        {
            m_attributes.Add(key, value);
        }

        public void AddString(string tag)
        {
            m_attributes.Add(tag, null);
        }

        public void OverrideCreatedOnUtc(DateTime createdUtc)
        {
            m_createdOnUtc = createdUtc;
        }

        public static EnvelopeBuilder CloneProperties(string newId, ImmutableEnvelope envelope)
        {
            if (newId == envelope.EnvelopeId)
            {
                throw new InvalidOperationException("Envelope cloned for modification should have new identity.");
            }
            var builder = new EnvelopeBuilder(newId);
            builder.OverrideCreatedOnUtc(envelope.CreatedOnUtc);
            builder.DeliverOnUtc(envelope.DeliverOnUtc);

            foreach (var attribute in envelope.GetAllAttributes())
            {
                builder.AddString(attribute.Key, attribute.Value);
            }
            return builder;
        }

        public MessageBuilder AddItem(ImmutableMessage message)
        {
            var item = new MessageBuilder(message.MappedType, message.Content);
            foreach (var attribute in message.GetAllAttributes())
            {
                item.AddAttribute(attribute.Key, attribute.Value);
            }
            Items.Add(item);
            return item;
        }

        public MessageBuilder AddItem<T>(T item)
        {
            // add KVPs after
            var t = typeof(T);
            if (t == typeof(object))
            {
                t = item.GetType();
            }

            var messageItemToSave = new MessageBuilder(t, item);
            Items.Add(messageItemToSave);

            return messageItemToSave;
        }

        public void DelayBy(TimeSpan span)
        {
            m_deliverOnUtc = DateTime.UtcNow + span;
        }

        public void DeliverOnUtc(DateTime deliveryDateUtc)
        {
            m_deliverOnUtc = deliveryDateUtc;
        }

        public ImmutableEnvelope Build()
        {
            var attributes = m_attributes.Select(p => new ImmutableAttribute(p.Key, p.Value)).ToArray();
            var items = new ImmutableMessage[Items.Count];

            for (int i = 0; i < items.Length; i++)
            {
                var save = Items[i];
                var attribs = save.Attributes.Select(p => new ImmutableAttribute(p.Key, p.Value)).ToArray();
                items[i] = new ImmutableMessage(save.MappedType, save.Content, attribs, i);
            }

            return new ImmutableEnvelope(EnvelopeId, attributes, items, m_deliverOnUtc, m_createdOnUtc);
        }
    }
}