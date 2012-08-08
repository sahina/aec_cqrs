using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Aec.Cqrs.Tests.Unit
{
    public class QueueWriterToFile : IQueueWriter
    {
        private readonly DirectoryInfo m_folder;
        private static long s_universalCounter;
        private readonly string m_suffix;

        public QueueWriterToFile(DirectoryInfo folder, string name)
        {
            m_folder = folder;
            m_suffix = Guid.NewGuid().ToString().Substring(0, 4);
            Name = name;
        }

        #region Implementation of IQueueWriter

        public string Name { get; private set; }

        public void PutMessage(ImmutableEnvelope envelope)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, envelope);

                File.WriteAllBytes(GenerateFileName(), stream.ToArray());
            }
        }

        #endregion

        #region Private Methods

        private string GenerateFileName()
        {
            var id = Interlocked.Increment(ref s_universalCounter);
            var fileName = string.Format("{0:yyyy-MM-dd-HH-mm-ss}-{1:00000000}-{2}", DateTime.UtcNow, id, m_suffix);

            return Path.Combine(m_folder.FullName, fileName);
        }

        #endregion
    }
}
