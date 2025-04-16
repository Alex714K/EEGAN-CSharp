namespace Logging;

public class Logger : IDisposable
{
    private readonly LogLevel _localLogLevel;
    private readonly Type _classType;
    
    private static readonly Semaphore Semaphore = new Semaphore(0, 1);
    
    #region StreamConfiguration
    private static StreamWriter _streamWriter;

    static Logger()
    {
        _streamWriter = ConfigureStreamWriter();
        
        Semaphore.Release();
    }

    private static StreamWriter ConfigureStreamWriter()
    {
        return new StreamWriter(GlobalLoggerVariables.LogFilePath, true)
        {
            AutoFlush = true,
        };
    }
    #endregion

    internal Logger(LogLevel? logLevel)
    {
        _localLogLevel = logLevel ?? LogLevel.Info;
        _classType = typeof(Logger);
    }
    
    internal Logger(LogLevel? logLevel, Type? classType)
    {
        _localLogLevel = logLevel ?? LogLevel.Info;
        _classType = classType ?? typeof(Logger);
    }

    private static void CheckAndChangeIfNeedLogFile()
    {
        var fileInfo = new FileInfo(GlobalLoggerVariables.LogFilePath);
        
        if (!fileInfo.Exists) fileInfo.Create();

        if (fileInfo.Length <= 4194304) return; // 4 MB
        
        // GlobalLoggerConfig.OldLogFilePath = GlobalLoggerConfig.LogFilePath;
        GlobalLoggerVariables.LogFilePath = CalculateNewLogFilePath();
        
        Semaphore.WaitOne();

        _streamWriter.Close();
        _streamWriter = new StreamWriter(GlobalLoggerVariables.LogFilePath, true)
        {
            AutoFlush = true
        };
        
        GlobalLoggerVariables.UpdateLoggerConfig();
        
        Semaphore.Release();
    }

    private void WriteLine(string message, LogLevel logLevel)
    {
        CheckAndChangeIfNeedLogFile();
        
        if (logLevel < _localLogLevel || logLevel < GlobalLoggerVariables.GlobalLogLevel) return;
        
        Semaphore.WaitOne();
        _streamWriter.WriteLine(LogMessage.ToString(message, logLevel, _classType));
        Semaphore.Release();
    }

    #region Log message support
    public void Trace(string message)
    {
        WriteLine(message, LogLevel.Trace);
    }

    public void Debug(string message)
    {
        WriteLine(message, LogLevel.Debug);
    }
    
    public void Info(string message)
    {
        WriteLine(message, LogLevel.Info);
    }

    public void Warning(string message)
    {
        WriteLine(message, LogLevel.Warning);
    }

    public void Error(string message)
    {
        WriteLine(message, LogLevel.Error);
    }
    
    /// <summary>
    /// Writing a <see cref="Fatal"/> message
    /// </summary>
    /// <param name="message">Text with information about log</param>
    public void Critical(string message) => Fatal(message);

    // ReSharper disable once MemberCanBePrivate.Global
    public void Fatal(string message)
    {
        WriteLine(message, LogLevel.Fatal);
    }

    public void Log(string message, LogLevel logLevel)
    {
        WriteLine(message, logLevel);
    }
    #endregion

    internal static string CalculateNewLogFilePath()
    {
        return $"data/{DateTime.Now:yyyyMMddHHmmss}.log";
    }

    public void Dispose()
    {
        _streamWriter.Dispose();
        GC.SuppressFinalize(this);
    }
}