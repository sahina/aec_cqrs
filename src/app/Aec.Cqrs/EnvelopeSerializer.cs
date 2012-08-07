using System.IO;
using System.Runtime.Serialization;

namespace Aec.Cqrs
{
    public class EnvelopeSerializer : IEnvelopeSerializer
    {
        #region Implementation of IEnvelopeSerializer

        public Stream SerializeToStream(ImmutableEnvelope envelope)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof (ImmutableEnvelope));
                serializer.WriteObject(stream, envelope);

                return stream;
            }
        }

        public byte[] SerializeToBytes(ImmutableEnvelope envelope)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableEnvelope DeserializeFromStream(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableEnvelope DeserializeFromBytes(byte[] buffer)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}