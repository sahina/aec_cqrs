namespace Aec.Cqrs
{
    public interface IQueueWriter
    {
        string Name { get; }
        void PutMessage(byte[] envelope);
        void PutMessage(ImmutableEnvelope envelope);
    }
}