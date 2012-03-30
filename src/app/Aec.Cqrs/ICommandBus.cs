namespace Aec.Cqrs
{
    public interface ICommandBus
    {
        void Send(ImmutableEnvelope envelope);
        void Send<T>(T command) where T : class, ICommand<IIdentity>;
        void SendAll<T>(params T[] commands) where T : class, ICommand<IIdentity>;
    }
}