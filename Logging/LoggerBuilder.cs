namespace Logging;

public class LoggerBuilder
{
    private Type? _classType;
    private LogLevel? _level;

    private static readonly Dictionary<Type, Logger> Loggers = new Dictionary<Type, Logger>();

    internal static Logger GetMyLogger()
    {
        if (Loggers.TryGetValue(typeof(GlobalLoggerVariables), out Logger? logger)) return logger;
        
        logger = new Logger(LogLevel.Info, typeof(GlobalLoggerVariables));
        Loggers.Add(typeof(GlobalLoggerVariables), logger);
        return logger;
    }

    /// <summary>
    /// <para>If you built logger before with this <paramref name="classType"/> then call this function. It's faster</para>
    /// Be careful! Will throw exception if you didn't build logger with this <paramref name="classType"/> before!
    /// </summary>
    /// <param name="classType">Type of class where you use this logger</param>
    /// <returns><see cref="Logger"/></returns>
    /// <exception cref="ArgumentException">If never built this logger before</exception>
    public static Logger GetLogger(Type classType)
    {
        if (Loggers.TryGetValue(classType, out Logger? value))
        {
            return value;
        }

        throw new ArgumentException("Unknown class type: " + classType.FullName);
    }

    public static LoggerBuilder Create()
    {
        return new LoggerBuilder();
    }

    /// <summary>
    /// <para>Building new logger, if he is never built before.</para>
    /// Better to build once and then use <see cref="GetLogger"/>. It's simply faster
    /// </summary>
    /// <returns>Logger with given class <see cref="Type"/> and <see cref="LogLevel"/></returns>
    /// <exception cref="ArgumentException">If <see cref="WithClassType"/> is never called</exception>
    public Logger Build()
    {
        if (_classType == null) throw new ArgumentException("Type is not added");
        
        if (Loggers.TryGetValue(_classType, out Logger? value)) return value;
        
        if (_level == null) _level = LogLevel.Info;
        
        var logger = new Logger(_level, _classType);
        
        Loggers.Add(_classType, logger);
        
        return logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public LoggerBuilder WithLogLevel(LogLevel level)
    {
        _level = level;
        return this;
    }

    /// <summary>
    /// <para>It's sets the name of logger, based on given class type.</para>
    /// Better to enter type of class, where you use <see cref="Logger"/>
    /// </summary>
    /// <param name="classType"><see cref="Type"/> of class</param>
    /// <returns></returns>
    public LoggerBuilder WithClassType(Type classType)
    {
        _classType = classType;
        return this;
    }

    /// <summary>
    /// <para>Setting log level of information in file. Information with log level lower than global log level will be ignored</para>
    /// <para>Base global log level is <see cref="LogLevel.Info"/></para>
    /// P.S. It's setting everywhere. Set once, use everywhere :)
    /// </summary>
    /// <param name="logLevel">What <see cref="LogLevel"/> you want to set</param>
    public static void SetGlobalLogLevel(LogLevel logLevel)
    {
        GlobalLoggerVariables.GlobalLogLevel = logLevel;
    }
}