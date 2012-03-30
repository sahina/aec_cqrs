namespace Aec.Cqrs
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent<IIdentity>
    {
        void Handle(TEvent theEvent);
    }
}