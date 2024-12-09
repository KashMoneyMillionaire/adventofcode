using System.Runtime.CompilerServices;

namespace Core;

public static class LoggingExtensions 
{
    private static bool IsDebug => Extensions.IsDebug;

    public static void Debug(this string val) 
    {
        if (IsDebug) Console.WriteLine(val);
    }

    public static T DebugMe<T>(this T val, [CallerArgumentExpression(nameof(val))] string? expr = default) 
    {
        if (IsDebug) Console.WriteLine($"{expr}: {val}");

        return val;
    }

    public static IEnumerable<T> DebugUs<T>(this IEnumerable<T> source, [CallerArgumentExpression(nameof(source))] string? expr = default) 
    {
        return source.DebugUs(x => x!.ToString()!, expr);
    }
    public static IEnumerable<T> DebugUs<T>(this IEnumerable<T> source, Func<T, string> mapper, [CallerArgumentExpression(nameof(source))] string? expr = default) 
    {
        $"Looking at collection: {expr}".Debug();

        return source.Select((s, i) => 
        {
            $"{i+1}: {mapper(s)}".Debug();
            return s;
        });
    }
}