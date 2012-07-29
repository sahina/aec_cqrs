using System;
using System.IO;

namespace Aec.Cqrs
{
    public sealed class FileDocumentReaderWriter<TKey, TItem> :
        IDocumentWriter<TKey, TItem>,
        IDocumentReader<TKey, TItem> where TKey : IIdentity
    {
        private readonly string m_folder;
        private readonly IDocumentStrategy m_strategy;

        public FileDocumentReaderWriter(string folder, IDocumentStrategy strategy)
        {
            m_folder = Path.Combine(folder, strategy.GetEntityBucket<TItem>()); ;
            m_strategy = strategy;
        }

        public void InitIfNeeded()
        {
            Directory.CreateDirectory(m_folder);
        }

        #region Implementation of IDocumentWriter<in TKey,TItem>

        public TItem AddOrUpdate(TKey key, Func<TItem> addFactory, Func<TItem, TItem> update)
        {
            var name = GetName(key);

            try
            {
                // This is fast and allows to have git-style subfolders in atomic strategy
                // to avoid NTFS performance degradation (when there are more than 
                // 10000 files per folder).
                var subfolder = Path.GetDirectoryName(name);

                if (subfolder != null && !Directory.Exists(subfolder))
                    Directory.CreateDirectory(subfolder);

                using (var file = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    TItem result;
                    if (file.Length == 0)
                    {
                        result = addFactory();
                    }
                    else
                    {
                        using (var mem = new MemoryStream())
                        {
                            file.CopyTo(mem);
                            mem.Seek(0, SeekOrigin.Begin);

                            var entity = m_strategy.Deserialize<TItem>(mem);
                            result = update(entity);
                        }
                    }

                    // some serializers have nasty habbit of closing the
                    // underling stream
                    using (var mem = new MemoryStream())
                    {
                        m_strategy.Serialize(result, mem);
                        var data = mem.ToArray();

                        file.Seek(0, SeekOrigin.Begin);
                        file.Write(data, 0, data.Length);

                        // truncate this file
                        file.SetLength(data.Length);
                    }

                    return result;
                }
            }
            catch (DirectoryNotFoundException)
            {
                var s = string.Format(
                    "Container '{0}' does not exist.",
                    m_folder);
                throw new InvalidOperationException(s);
            }
        }

        public bool TryDelete(TKey key)
        {
            var name = GetName(key);

            if (File.Exists(name))
            {
                File.Delete(name);
                return true;
            }

            return false;
        }

        #endregion

        #region Implementation of IDocumentReader<in TKey,TItem>

        public bool TryGet(TKey key, out TItem item)
        {
            item = default(TItem);
            try
            {
                var name = GetName(key);

                if (!File.Exists(name))
                    return false;

                using (var stream = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    item = m_strategy.Deserialize<TItem>(stream);
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                // if file happened to be deleted between the moment of check and actual read.
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        private string GetName(TKey key)
        {
            return Path.Combine(m_folder, m_strategy.GetEntityLocation(typeof(TItem), key));
        }

        #endregion
    }
}