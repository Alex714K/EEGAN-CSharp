using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Logging;

internal static class GlobalLoggerVariables
{
    private const string ConfigFilePath = "data/loggerConfig.json";
    internal static LogLevel GlobalLogLevel = LogLevel.Info;
    
    private static readonly Logger Logger = LoggerBuilder.GetMyLogger();

    private static IConfiguration Configuration { get; }

    internal static string LogFilePath;

    // internal static string OldLogFilePath;

    static GlobalLoggerVariables()
    {
        Configuration = GetConfiguration();

        LogFilePath = GetLogFilePath(Configuration["LogFilePath"]);
        
        // OldLogFilePath = LogFilePath;
    }

    private static IConfiguration GetConfiguration()
    {
        if (!File.Exists(ConfigFilePath))
            CreateLoggerConfigFile();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(ConfigFilePath, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        return configuration;
    }

    private static string GetLogFilePath(string? logFilePath)
    {
        if (logFilePath is null or "app.log") return Logger.CalculateNewLogFilePath();
        
        var fileInfo = new FileInfo(logFilePath);
        
        if (!fileInfo.Exists) fileInfo.Create();
        
        return fileInfo is { Exists: true, Length: <= 4194304 }
            ? fileInfo.FullName : Logger.CalculateNewLogFilePath();
    }

    #region ConfigFile

    private static void CreateLoggerConfigFile()
    {
        WriteConfigInConfigFile();
        Console.WriteLine("Created loggerConfig.json");
    }
    
    internal static void UpdateLoggerConfig()
    {
        WriteConfigInConfigFile();
        Logger.Info("Updated loggerConfig.json");
    }

    private static void WriteConfigInConfigFile()
    {
        var config = new
        {
            LogFilePath = Logger.CalculateNewLogFilePath()
        };

        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(ConfigFilePath, json);
    }
    #endregion
}