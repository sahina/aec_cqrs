using System.IO;

namespace Aec.Cqrs
{
    public sealed class FileStorageConfig
    {
        public DirectoryInfo Folder { get; private set; }
        public string AccountName { get; private set; }

        public string FullPath
        {
            get { return Folder.FullName; }
        }

        public FileStorageConfig(DirectoryInfo folder, string accountName)
        {
            Folder = folder;
            AccountName = accountName;
        }

        public FileStorageConfig SubFolder(string path, string account = null)
        {
            return new FileStorageConfig(new DirectoryInfo(Path.Combine(Folder.FullName, path)),
                                         account ?? AccountName + "-" + path);
        }

        public void Wipe()
        {
            if (Folder.Exists)
                Folder.Delete(true);
        }

        public void EnsureDirectory()
        {
            Folder.Create();
        }

        public void Reset()
        {
            Wipe();
            EnsureDirectory();
        }
    }
}