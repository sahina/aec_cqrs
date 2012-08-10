namespace Aec.Infrastructure.Framework.Logging
{
    public interface ILogger
    {
        void LogInfo(object info);
        void LogWarn(object info);
        void LogDebug(object info);
        void LogError(object info);
        void LogFatal(object info);
    }
}
