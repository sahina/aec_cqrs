namespace Aec.Infrastructure.Framework.Security
{
    public interface IHashService
    {
        string Hash(string clearText);
        bool Verify(string clearText, string hash);
    }
}
