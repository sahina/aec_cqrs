using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Aec.Cqrs
{
    public class FileDocumentStore : IDocumentStore
    {
        private readonly string m_folderPath;
        private readonly IDocumentStrategy m_strategy;
        private readonly HashSet<Tuple<Type, Type>> m_initialized = new HashSet<Tuple<Type, Type>>();

        public FileDocumentStore(string folderPath, IDocumentStrategy strategy)
        {
            m_folderPath = folderPath;
            m_strategy = strategy;
        }

        #region Implementation of IDocumentStore

        public IDocumentReader<TKey, TItem> GetReader<TKey, TItem>() where TKey : IIdentity
        {
            return new FileDocumentReaderWriter<TKey, TItem>(m_folderPath, m_strategy);
        }

        public IDocumentWriter<TKey, TItem> GetWriter<TKey, TItem>() where TKey : IIdentity
        {
            var container = new FileDocumentReaderWriter<TKey, TItem>(m_folderPath, m_strategy);
            if (m_initialized.Add(Tuple.Create(typeof(TKey), typeof(TItem))))
            {
                container.InitIfNeeded();
            }
            return container;
        }

        public void Reset(string bucket)
        {
            var path = Path.Combine(m_folderPath, bucket);

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }

        public void WriteContents(string bucket, IEnumerable<SavedRecord> records)
        {
            var buck = Path.Combine(m_folderPath, bucket);

            if (!Directory.Exists(buck))
                Directory.CreateDirectory(buck);

            foreach (var pair in records)
            {
                var recordPath = Path.Combine(buck, pair.Version.ToString(CultureInfo.InvariantCulture));
                var path = Path.GetDirectoryName(recordPath) ?? "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var stream = new MemoryStream();
                m_strategy.Serialize(pair.Content, stream);

                File.WriteAllBytes(recordPath, stream.ToArray());
            }
        }

        public IDocumentStrategy Strategy
        {
            get { return m_strategy; }
        }

        #endregion

        public override string ToString()
        {
            return new Uri(Path.GetFullPath(m_folderPath)).AbsolutePath;
        }
    }
}