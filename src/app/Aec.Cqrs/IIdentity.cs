namespace Aec.Cqrs
{
    public interface IIdentity
    {
        string GetIdenfitier();
        string GetTag();
    }
}