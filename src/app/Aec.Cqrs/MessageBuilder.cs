using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public sealed class MessageBuilder
    {
        internal readonly IDictionary<string, string> Attributes = new Dictionary<string, string>();

        public readonly Type MappedType;
        public readonly object Content;

        public MessageBuilder(Type mappedType, object content)
        {
            MappedType = mappedType;
            Content = content;
        }

        public void AddAttribute(string key, string value)
        {
            Attributes.Add(key, value);
        }
    }
}