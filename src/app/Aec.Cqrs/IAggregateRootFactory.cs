namespace Aec.Cqrs
{
    public interface IAggregateRootFactory
    {
        Applied Execute(params ICommand<IIdentity>[] commands);
        void ExecuteStoreDispatch(params ICommand<IIdentity>[] commands);
        void AppendToStorage(IIdentity id, Applied applied);
        void DispatchEvents(Applied applied);
    }
}