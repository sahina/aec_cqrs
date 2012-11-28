namespace Aec.Cqrs
{
    public interface IAggregateState
    {
        void Mutate(IEvent<IIdentity> e);
    }
}