using System;
using System.IO;
using ServiceStack.Text;

namespace Aec.Cqrs
{
    public class DefaultJsonDocumentStrategy : IDocumentStrategy
    {
        public string GetEntityBucket<T>()
        {
            return "doc-" + typeof(T).Name.ToLowerInvariant();
        }

        public virtual string GetEntityLocation(Type entity, object key)
        {
            if (key is IIdentity)
                return IdentityConvert.ToStream((IIdentity)key);

            return key.ToString().ToLowerInvariant();
        }

        public virtual void Serialize<TEntity>(TEntity entity, Stream stream)
        {
            var s = JsonSerializer.SerializeToString(entity);
            s = JsvFormatter.Format(s);

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(s);
            }
        }

        public virtual TEntity Deserialize<TEntity>(Stream stream)
        {
            return JsonSerializer.DeserializeFromStream<TEntity>(stream);
        }
    }
}