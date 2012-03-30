namespace Aec.Cqrs
{
    public interface IRecordStorageFactory
    {
        IRecordStorage GetOrCreateStorage(IIdentity id);
    }
}