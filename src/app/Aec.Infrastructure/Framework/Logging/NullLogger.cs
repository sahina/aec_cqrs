namespace Aec.Infrastructure.Framework.Logging
{
    public class NullLogger : ILogger
    {
        #region Implementation of ILogger

        public void LogInfo(object info)
        {
        }

        public void LogWarn(object info)
        {
        }

        public void LogDebug(object info)
        {
        }

        public void LogError(object info)
        {
        }

        public void LogFatal(object info)
        {
        }

        #endregion
    }
}