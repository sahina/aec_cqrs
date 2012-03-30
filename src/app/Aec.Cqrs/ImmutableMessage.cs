using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    public sealed class ImmutableMessage
    {
        public readonly Type MappedType;
        public readonly object Content;
        public readonly int Index;
        readonly ImmutableAttribute[] m_attributes;

        public ICollection<ImmutableAttribute> GetAllAttributes()
        {
            return m_attributes;
        }

        public bool TryGetAttribute(string name, out string result)
        {
            foreach (var attribute in m_attributes)
            {
                if (attribute.Key == name)
                {
                    result = attribute.Value;
                    return true;
                }
            }
            result = null;
            return false;
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


        public ImmutableMessage(Type mappedType, object content, ImmutableAttribute[] attributes, int index)
        {
            MappedType = mappedType;
            Index = index;
            Content = content;
            m_attributes = attributes;
        }

        public override string ToString()
        {
            return string.Format("[{0}]", Content);
        }
    }
}