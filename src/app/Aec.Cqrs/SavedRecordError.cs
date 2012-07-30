using System;

namespace Aec.Cqrs
{
    /// <summary>
    /// Is pubslished whenever saved record error is observed
    /// </summary>
    public class SavedRecordError : ISystemEvent
    {
        public readonly Exception Exception;
        public readonly string ID;
        public readonly long Version;
        public readonly object Content;

        public SavedRecordError(Exception exception, string id, long version, object content)
        {
            Exception = exception;
            ID = id;
            Version = version;
            Content = content;
        }
    }
}