using System.IO;

namespace Aec.Cqrs
{
    public class FileRecordStorageFactory : IRecordStorageFactory
    {
        private readonly IRecordStorageConfig m_config;

        public FileRecordStorageFactory(IRecordStorageConfig config)
        {
            m_config = config;

            InitStorageIfNeeded();
        }

        #region Implementation of IRecordStorageFactory

        public IRecordStorage GetOrCreateStorage(IIdentity id)
        {
            var path = Path.Combine((string) m_config.BuildConfiguration(), IdentityConvert.ToStream(id));

            return new FileRecordStorage(id, new DirectoryInfo(path));
        }

        #endregion

        #region Private Methods

        private void InitStorageIfNeeded()
        {
            var path = (string) m_config.BuildConfiguration();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #endregion
    }
}