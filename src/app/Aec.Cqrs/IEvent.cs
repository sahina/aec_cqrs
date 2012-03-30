namespace Aec.Cqrs
{
    public interface IEvent<out TIdentity> : IMessage where TIdentity : IIdentity
    {
        TIdentity ID { get; }
    }
}