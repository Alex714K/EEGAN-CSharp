namespace Logging;

internal class LogMessage
{
    public static string ToString(string message, LogLevel logLevel, Type classType)
    {
        return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {classType.FullName} - {logLevel.ToString()} - {message}";
    }
}