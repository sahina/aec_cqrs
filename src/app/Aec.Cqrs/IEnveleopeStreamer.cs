namespace Aec.Cqrs
{
    /// <summary>
    /// Is responsible for reading-writing message envelopes either as
    /// data or references to data (in case envelope does not fit media)
    /// </summary>
    public interface IEnvelopeStreamer
    {
        /// <summary>
        /// Saves message envelope as array of bytes.
        /// </summary>
        /// <param name="envelope">The message envelope.</param>
        /// <returns></returns>
        byte[] SaveEnvelopeData(ImmutableEnvelope envelope);

        /// <summary>
        /// Reads the buffer as message envelope
        /// </summary>
        /// <param name="buffer">The buffer to read.</param>
        /// <returns>mes    sage envelope</returns>
        ImmutableEnvelope ReadAsEnvelopeData(byte[] buffer);
    }
}
