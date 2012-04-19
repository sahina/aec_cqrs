namespace Aec.Cqrs
{
    public interface ICommand<out TIdentity> : IMessage where TIdentity : IIdentity
    {
        TIdentity Identity { get; }
    }
}