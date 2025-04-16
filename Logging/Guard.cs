namespace Logging;

#nullable disable
internal static class Guard
{
    internal static T ThrowIfNull<T>(T arg, string param = "") where T : class
    {
        return arg ?? throw new ArgumentNullException(string.IsNullOrEmpty(param) ? typeof (T).Name : param);
    }
    
    internal static string ThrowIfNullOrEmpty(string arg, string param = "")
    {
        return !string.IsNullOrEmpty(arg) ? arg : throw new ArgumentNullException(string.IsNullOrEmpty(param) ? nameof (arg) : param);
    }
}