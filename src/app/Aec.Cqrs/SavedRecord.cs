namespace Aec.Cqrs
{
    public class SavedRecord
    {
        public long Version { get; private set; }
        public object Content { get; private set; }

        public SavedRecord(long version, object content)
        {
            Version = version;
            Content = content;
        }
    }
}