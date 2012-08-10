namespace Aec.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}