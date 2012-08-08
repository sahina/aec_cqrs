namespace Aec.Cqrs.Tests.Unit
{
    public interface IQueueWriterFactory
    {
        string Endpoint { get; }
        IQueueWriter GetWriteQueue(string queueName);
    }
}