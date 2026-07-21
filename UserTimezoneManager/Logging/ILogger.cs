namespace Ronin.XrmToolbox.UserTimezoneManager.Logging
{
    /// <summary>Abstraction over the log output panel so services remain UI-independent.</summary>
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message);
        void Clear();
    }
}
