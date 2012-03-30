namespace Aec.Cqrs
{
    public interface ICommandHandler<in TDomainCommand> where TDomainCommand : ICommand<IIdentity>
    {
        void Handle(TDomainCommand command);
    }
}