namespace Aec.Cqrs
{
    public interface IAggregateState
    {
        void Apply(IEvent<IIdentity> e);
    }
}