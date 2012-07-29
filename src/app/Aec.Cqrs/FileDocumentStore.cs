using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Resets all documents in storage
        /// </summary>
        public void ResetAll()
        {
            if (Directory.Exists(m_folderPath))
                Directory.Delete(m_folderPath, true);
            
            Directory.CreateDirectory(m_folderPath);
        }

        public void WriteContents(string bucket, IEnumerable<DocumentRecord> records)
        {
            var buck = Path.Combine(m_folderPath, bucket);

            if (!Directory.Exists(buck))
                Directory.CreateDirectory(buck);
            
            foreach (var pair in records)
            {
                var recordPath = Path.Combine(buck, pair.Key);

                var path = Path.GetDirectoryName(recordPath) ?? "";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllBytes(recordPath, pair.Read());
            }
        }

        /// <summary>
        /// Gets enumerable saved s=records in the given bucket (partition)
        /// </summary>
        /// <param name="bucket">Bucket (partition)</param>
        /// <returns>List of saved records</returns>
        public IEnumerable<DocumentRecord> EnumerateContents(string bucket)
        {
            var full = Path.Combine(m_folderPath, bucket);
            var dir = new DirectoryInfo(full);

            if (!dir.Exists) yield break;

            var fullFolder = dir.FullName;

            foreach (var info in dir.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                var fullName = info.FullName;
                var path = fullName.Remove(0, fullFolder.Length + 1).Replace(Path.DirectorySeparatorChar, '/');

                yield return new DocumentRecord(path, () => File.ReadAllBytes(fullName));
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