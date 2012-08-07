using System;
using System.IO;
using System.Threading;

namespace Aec.Cqrs.Tests.Unit
{
    public class QueueWriterToFile : IQueueWriter
    {
        private readonly DirectoryInfo m_folder;
        private readonly IEnvelopeSerializer m_serializer;
        private static long s_universalCounter;
        private readonly string m_suffix;

        public QueueWriterToFile(DirectoryInfo folder, string name)
        {
            m_folder = folder;
            m_suffix = Guid.NewGuid().ToString().Substring(0, 4);
            Name = name;
            m_serializer = new EnvelopeSerializer();
        }
        public QueueWriterToFile(DirectoryInfo folder, string name, IEnvelopeSerializer serializer)
        {
            m_folder = folder;
            m_serializer = serializer;
            m_suffix = Guid.NewGuid().ToString().Substring(0, 4);
            Name = name;
        }

        #region Implementation of IQueueWriter

        public string Name { get; private set; }

        public void PutMessage(byte[] envelope)
        {
            File.WriteAllBytes(GenerateFileName(), envelope);
        }

        public void PutMessage(ImmutableEnvelope envelope)
        {
            var bytes = m_serializer.SerializeToBytes(envelope);

            File.WriteAllBytes(GenerateFileName(), bytes);
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
