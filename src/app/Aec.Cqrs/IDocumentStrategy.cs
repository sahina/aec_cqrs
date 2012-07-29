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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="stream"></param>
        void Serialize<TEntity>(TEntity entity, Stream stream);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        TEntity Deserialize<TEntity>(Stream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string GetEntityBucket<TEntity>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetEntityLocation(Type entity, object key);
    }
}