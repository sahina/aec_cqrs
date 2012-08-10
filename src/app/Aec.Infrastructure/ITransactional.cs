namespace Aec.Infrastructure
{
    public interface ITransactional
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}