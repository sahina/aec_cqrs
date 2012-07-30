using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Aec.Cqrs
{
    public class FileRecordStorage : IRecordStorage
    {
        private readonly IIdentity m_id;
        private readonly DirectoryInfo m_directoryInfo;
        private readonly ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim();
        private readonly ConcurrentDictionary<string, SavedRecord[]> m_cache =
            new ConcurrentDictionary<string, SavedRecord[]>();

        public FileRecordStorage(IIdentity id, DirectoryInfo directoryInfo)
        {
            m_id = id;
            m_directoryInfo = directoryInfo;

            EnsureDirectoryExists();
            LoadCache();
        }

        #region Implementation of IRecordStorage

        /// <summary>
        /// Gets all the records for the given version and expected count.
        /// </summary>
        /// <param name="afterVersion">We get records after the given version number</param>
        /// <param name="maxCount">Number of records to fetch.</param>
        /// <returns>Collection of records.</returns>
        public IEnumerable<SavedRecord> GetRecords(long afterVersion, int maxCount)
        {
            if (afterVersion < 0)
                throw new ArgumentOutOfRangeException("afterVersion", "Must be zero or greater.");

            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException("maxCount", "Must be more than zero.");

            // afterVersion + maxCount > long.MaxValue, but transformed to avoid overflow
            if (afterVersion > long.MaxValue - maxCount)
                throw new ArgumentOutOfRangeException("maxCount", "Version will exceed long.MaxValue.");

            var records = m_cache[m_id.GetIdenfitier()]
                .Skip((int)afterVersion)
                .Take(maxCount)
                .ToArray();

            return records;
        }

        /// <summary>
        /// Appends the given content to storage.
        /// </summary>
        /// <param name="content">Content to append to storage.</param>
        /// <returns>True if save is successfull, otherwise false.</returns>
        public bool TryAppend(object[] content)
        {
            try
            {
                m_lock.EnterWriteLock();

                var streamName = IdentityConvert.ToStream(m_id);
                var lastVersion = m_cache.ContainsKey(streamName) ? m_cache[streamName].Last().Version : 0;

                content.ToList().ForEach(item =>
                {
                    var version = ++lastVersion;

                    PersistInFile(streamName, version, item);
                    AddToCache(streamName, version, item);
                });

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_lock.ExitWriteLock();
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void PersistInFile(string id, long version, object content)
        {
            FileStream writer = null;

            try
            {
                using (var sha1 = new SHA1Managed())
                {
                    using (var memory = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(memory, content);

                        var bytes = memory.ToArray();

                        writer = EnsureWriterExists(version);
                        writer.Write(bytes, 0, bytes.Length);
                    }

                    writer.Write(sha1.Hash, 0, sha1.Hash.Length);
                    writer.Flush(true);
                }
            }
            catch (Exception ex)
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }

                SystemObserver.Notify(new SavedRecordError(ex, id, version, content));
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        private void AddToCache(string streamName, long version, object content)
        {
            var record = new SavedRecord(version, content);

            m_cache.AddOrUpdate(streamName, r => new[] { record }, (s, records) => ImmutableAdd(records, record));
        }

        private FileStream EnsureWriterExists(long version)
        {
            var fileName = string.Format("{0:00000000}-{1:yyyy-MM-dd-HHmmss}.dat", version, DateTime.UtcNow);

            return File.OpenWrite(Path.Combine(m_directoryInfo.FullName, fileName));
        }

        private void EnsureDirectoryExists()
        {
            if (!m_directoryInfo.Exists)
                m_directoryInfo.Create();
        }

        private static T[] ImmutableAdd<T>(T[] source, T item)
        {
            var copy = new T[source.Length + 1];

            Array.Copy(source, copy, source.Length);
            copy[source.Length] = item;

            return copy;
        }

        private void LoadCache()
        {
            try
            {
                m_lock.EnterWriteLock();

                //foreach (var record in ReadHistory())
                  //  AddToCache(record., record.Version, record.Content);
            }
            finally
            {
                m_lock.ExitWriteLock();
            }
        }

        private IEnumerable<SavedRecord> ReadHistory()
        {
            var result = new List<SavedRecord>();
            var files = m_directoryInfo.EnumerateFiles("*.dat");

            foreach (var file in files.OrderBy(f => f.Name))
            {
                if (file.Length == 0)
                    file.Delete();

                m_cache.TryAdd(file.Name, new SavedRecord[] { });

                using (var reader = file.OpenRead())
                using (var binary = new BinaryReader(reader, Encoding.UTF8))
                {
                    var length = file.Length;
                    var totalBytes = binary.ReadBytes((int)length);

                    using (var memory = new MemoryStream(totalBytes))
                    {
                        memory.Position = 0;

                        var formatter = new BinaryFormatter();
                        result.Add((SavedRecord)formatter.Deserialize(memory));
                    }
                }
            }

            return result;
        }

        #endregion
    }
}