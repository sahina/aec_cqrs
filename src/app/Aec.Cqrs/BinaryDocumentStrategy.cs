using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Aec.Cqrs
{
    public class BinaryDocumentStrategy : DefaultJsonDocumentStrategy
    {
        private readonly BinaryFormatter m_formatter;

        public BinaryDocumentStrategy()
        {
            m_formatter = new BinaryFormatter();
        }

        public override string GetEntityLocation(Type entity, object key)
        {
            var identity = key as IIdentity;

            if (identity != null)
                return IdentityConvert.ToStream(identity) + ".bin";

            return key.ToString().ToLowerInvariant() + ".bin";
        }

        public override void Serialize<TEntity>(TEntity entity, Stream stream)
        {
            m_formatter.Serialize(stream, entity);
        }

        public override TEntity Deserialize<TEntity>(Stream stream)
        {
            return (TEntity) m_formatter.Deserialize(stream);
        }
    }
}