using System.IO;

namespace Aec.Cqrs
{
    /// <summary>
    /// Serializes and deserializes an envelope
    /// </summary>
    public interface IEnvelopeSerializer
    {
        Stream SerializeToStream(ImmutableEnvelope envelope);
        byte[] SerializeToBytes(ImmutableEnvelope envelope);
        ImmutableEnvelope DeserializeFromStream(Stream stream);
        ImmutableEnvelope DeserializeFromBytes(byte[] buffer);
    }
}