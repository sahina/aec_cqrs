namespace Aec.Cqrs
{
    public interface IApplicationService
    {
        void Execute(ICommand<IIdentity> command);
    }
}