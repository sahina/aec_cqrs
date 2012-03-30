using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class MemoryRecordStorageFactory : IRecordStorageFactory
    {
        private static readonly Dictionary<IIdentity, MemoryRecordStorage> s_storages =
            new Dictionary<IIdentity, MemoryRecordStorage>();

        #region Implementation of IRecordStorageFactory

        public IRecordStorage GetOrCreateStorage(IIdentity id)
        {
            if (!s_storages.ContainsKey(id))
                s_storages.Add(id, new MemoryRecordStorage(id));

            return s_storages[id];
        }

        #endregion
    }
}