using System;
using System.IO;

namespace Aec.Cqrs
{
    /// <summary>
    /// Logically this strategy contains two different aspects: serialization and location.
    /// However it is more convenient to keep them in one interface, since they are frequently
    /// passed together (e.g.: in projection management code)
    /// </summary>
    public interface IDocumentStrategy
    {
        void Serialize<TEntity>(TEntity entity, Stream stream);
        TEntity Deserialize<TEntity>(Stream stream);

        string GetEntityBucket<TEntity>();
        string GetEntityLocation(Type entity, object key);
    }
}