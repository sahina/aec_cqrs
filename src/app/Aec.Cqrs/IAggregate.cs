namespace Aec.Cqrs
{
    public interface IAggregate<in TIdentity> where TIdentity : IIdentity
    {
        void Execute(ICommand<TIdentity> c);
    }
}