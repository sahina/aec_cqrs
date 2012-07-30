using System;

namespace Aec.Cqrs
{
    [Serializable]
    public class SavedRecord
    {
        public string Key { get; private set; }
        public long Version { get; private set; }
        public object Content { get; private set; }

        public SavedRecord(string key, long version, object content)
        {
            Key = key;
            Version = version;
            Content = content;
        }
    }
}