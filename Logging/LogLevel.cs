using System.Globalization;
// ReSharper disable RedundantCast

#nullable disable
namespace Logging;

/// <summary>Defines available log levels.</summary>
/// <remarks>
/// Log levels ordered by severity:<br />
/// - <see cref="LogLevel.Trace" /> (Ordinal = 0) : Most verbose level. Used for development and seldom enabled in production.<br />
/// - <see cref="LogLevel.Debug" /> (Ordinal = 1) : Debugging the application behavior from internal events of interest.<br />
/// - <see cref="LogLevel.Info" />  (Ordinal = 2) : Information that highlights progress or application lifetime events.<br />
/// - <see cref="LogLevel.Warning" />  (Ordinal = 3) : Warnings about validation issues or temporary failures that can be recovered.<br />
/// - <see cref="LogLevel.Error" /> (Ordinal = 4) : Errors where functionality has failed or <see cref="T:System.Exception" /> have been caught.<br />
/// - <see cref="LogLevel.Fatal" /> (Ordinal = 5) : Most critical level. Application is about to abort.<br />
/// </remarks>
public sealed class LogLevel :
    IEquatable<LogLevel>,
    IComparable<LogLevel>,
    IComparable,
    IFormattable
{
    /// <summary>Trace log level (Ordinal = 0)</summary>
    /// <remarks>
    /// Most verbose level. Used for development and seldom enabled in production.
    /// </remarks>
    public static readonly LogLevel Trace = new LogLevel(nameof(Trace), 0);
    
    /// <summary>Debug log level (Ordinal = 1)</summary>
    /// <remarks>
    /// Debugging the application behavior from internal events of interest.
    /// </remarks>
    public static readonly LogLevel Debug = new LogLevel(nameof(Debug), 1);

    /// <summary>Info log level (Ordinal = 2)</summary>
    /// <remarks>
    /// Information that highlights progress or application lifetime events.
    /// </remarks>
    public static readonly LogLevel Info = new LogLevel(nameof(Info), 2);
    
    /// <summary>Warn log level (Ordinal = 3) (better use Warning)</summary>
    /// <remarks>
    /// Warnings about validation issues or temporary failures that can be recovered.
    /// </remarks>
    [Obsolete("Better use LogLevel.Warning instead.")]
    public static readonly LogLevel Warn = new LogLevel(nameof(Warn), 3);

    /// <summary>Warning log level (Ordinal = 3)</summary>
    /// <remarks>
    /// Warnings about validation issues or temporary failures that can be recovered.
    /// </remarks>
    public static readonly LogLevel Warning = new LogLevel(nameof(Warning), 3);

    /// <summary>Error log level (Ordinal = 4)</summary>
    /// <remarks>
    /// Errors where functionality has failed or <see cref="T:System.Exception" /> have been caught.
    /// </remarks>
    public static readonly LogLevel Error = new LogLevel(nameof(Error), 4);

    /// <summary>Fatal log level (Ordinal = 5)</summary>
    /// <remarks>Most critical level. Application is about to abort.</remarks>
    public static readonly LogLevel Fatal = new LogLevel(nameof(Fatal), 5);

    /// <summary>Off log level (Ordinal = 6)</summary>
    public static readonly LogLevel Off = new LogLevel(nameof(Off), 6);

    public static readonly IList<LogLevel> AllLevels = new List<LogLevel>()
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
        Off
    }.AsReadOnly();

    public static readonly IList<LogLevel> AllLoggingLevels = new List<LogLevel>()
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }.AsReadOnly();


    private readonly string _name;
    private readonly int _ordinal;

    public static LogLevel MaxLevel => LogLevel.Fatal;

    public static LogLevel MinLevel => LogLevel.Trace;

    /// <summary>
    /// Initializes a new instance of <see cref="LogLevel" />.
    /// </summary>
    /// <param name="name">The <see cref="LogLevel"/> name.</param>
    /// <param name="ordinal">The <see cref="LogLevel"/> ordinal number.</param>
    private LogLevel(string name, int ordinal)
    {
        this._name = name;
        this._ordinal = ordinal;
    }

    /// <summary>Gets the name of the <see cref="LogLevel"/>.</summary>
    public string Name => this._name;

    /// <summary>Gets the ordinal of the <see cref="LogLevel"/>.</summary>
    public int Ordinal => this._ordinal;


    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is equal to the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal == logLevel2.Ordinal</c>.</returns>
    public static bool operator ==(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return true;
        LogLevel logLevel = logLevel1 ?? Off;
        return logLevel.Equals(logLevel2);
    }

    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is not equal to the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal != logLevel2.Ordinal</c>.</returns>
    public static bool operator !=(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return false;
        LogLevel logLevel = logLevel1;
        if (logLevel == null)
            logLevel = Off;
        return !logLevel.Equals(logLevel2);
    }

    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is greater than the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal &gt; logLevel2.Ordinal</c>.</returns>
    public static bool operator >(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return false;
        LogLevel logLevel = logLevel1;
        if (logLevel == null)
            logLevel = Off;
        return logLevel.CompareTo(logLevel2) > 0;
    }

    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is less than the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal &lt; logLevel2.Ordinal</c>.</returns>
    public static bool operator <(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return false;
        LogLevel logLevel = logLevel1;
        if (logLevel == null)
            logLevel = Off;
        return logLevel.CompareTo(logLevel2) < 0;
    }

    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is greater than or equal to the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal &gt;= logLevel2.Ordinal</c>.</returns>
    public static bool operator >=(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return true;
        LogLevel logLevel = logLevel1;
        if (logLevel == null)
            logLevel = Off;
        return logLevel.CompareTo(logLevel) >= 0;
    }

    /// <summary>
    /// Compares two <see cref="LogLevel" /> objects
    /// and returns a value indicating whether
    /// the first one is less than or equal to the second one.
    /// </summary>
    /// <param name="logLevel1">The first level.</param>
    /// <param name="logLevel2">The second level.</param>
    /// <returns>The value of <c>logLevel1.Ordinal &lt;= logLevel2.Ordinal</c>.</returns>
    public static bool operator <=(LogLevel logLevel1, LogLevel logLevel2)
    {
        if ((object)logLevel1 == (object)logLevel2)
            return true;
        LogLevel logLevel = logLevel1;
        if (logLevel == null)
            logLevel = Off;
        return logLevel.CompareTo(logLevel) <= 0;
    }

    /// <summary>
    /// Gets the <see cref="LogLevel" /> that corresponds to the specified ordinal.
    /// </summary>
    /// <param name="ordinal">The ordinal.</param>
    /// <returns>The <see cref="LogLevel" /> instance. For 0 it returns <see cref="LogLevel.Trace" />, 1 gives <see cref="LogLevel.Debug" /> and so on.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static LogLevel FromOrdinal(int ordinal)
    {
        return ordinal switch
        {
            0 => Trace,
            1 => Debug,
            2 => Info,
            3 => Warning,
            4 => Error,
            5 => Fatal,
            6 => Off,
            _ => throw new ArgumentException("Unknown loglevel: " + ordinal.ToString() + ".", nameof(ordinal))
        };
    }

    /// <summary>
    /// Returns the <see cref="LogLevel" /> that corresponds to the supplied <see langword="string" />.
    /// </summary>
    /// <param name="levelName">The textual representation of the log level.</param>
    /// <returns>The enumeration value.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static LogLevel FromString(string levelName)
    {
        Guard.ThrowIfNull(levelName, nameof(levelName));
        return (levelName[0].ToString().ToUpper() + levelName[1..].ToLower()) switch
        {
            "Trace" => Trace,
            "Debug" => Debug,
            "Info" => Info,
            "Warning" => Warning,
            "Error" => Error,
            "Fatal" => Fatal,
            "Off" => Off,
            "Warn" => Warning,
            "Information" => Info,
            _ => throw new ArgumentException("Unknown log level: " + levelName, nameof(levelName))
        };
    }

    /// <summary>Returns a string representation of the log level.</summary>
    /// <returns>LogLevel name.</returns>
    public override string ToString() => this._name;

    /// <inheritdoc cref="ToString()"/>
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return string.IsNullOrEmpty(format) ? this._name : this._ordinal.ToString(CultureInfo.InvariantCulture);
    }

    public override int GetHashCode()
    {
        return this._ordinal;
    }

    /// <inheritdoc cref="Equals(LogLevel)"/>
    public override bool Equals(object other)
    {
        return Equals((LogLevel)other);
    }

    /// <summary>
    /// Determines whether the specified <see cref="LogLevel" /> instance is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="LogLevel" /> to compare with this instance.</param>
    /// <returns>Value of <c>true</c> if the specified <see cref="LogLevel" /> is equal to
    /// this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(LogLevel other)
    {
        int? ordinal2 = other?._ordinal;
        int valueOrDefault = ordinal2.GetValueOrDefault();
        return this._ordinal == valueOrDefault && ordinal2.HasValue;
    }

    /// <inheritdoc cref="CompareTo(LogLevel)"/>
    public int CompareTo(object other) => CompareTo((LogLevel)other);

    /// <summary>
    /// Compares the level to the other <see cref="LogLevel" /> object.
    /// </summary>
    /// <param name="other">The other object.</param>
    /// <returns>
    /// A value less than zero when this logger's <see cref="LogLevel.Ordinal" /> is
    /// less than the other logger's ordinal, 0 when they are equal and
    /// greater than zero when this ordinal is greater than the
    /// other ordinal.
    /// </returns>
    public int CompareTo(LogLevel other)
    {
        LogLevel logLevel = other ?? Off;
        int ordinal2 = logLevel._ordinal;
        return this._ordinal - ordinal2;
    }
}