namespace Aec.Cqrs
{
    public interface IQueueWriterFactory
    {
        string Endpoint { get; }
        IQueueWriter GetWriteQueue(string queueName);
    }
}