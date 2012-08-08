namespace Aec.Cqrs
{
    public interface IQueueWriter
    {
        string Name { get; }
        void PutMessage(ImmutableEnvelope envelope);
    }
}