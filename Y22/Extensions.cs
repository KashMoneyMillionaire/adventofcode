using System.Collections;

namespace Y22;

public static class Extensions
{
    public static void Print(this string value, string prefix) => Console.WriteLine($"{prefix}{value}");

    public static IEnumerable<string> ReadLines(this string input, bool skipEmptyLines = true, string? splitOn = null)
        => input.Split(splitOn ?? Environment.NewLine)
                .Where(l => skipEmptyLines && !string.IsNullOrWhiteSpace(l))
                .Where(l => !l.StartsWith("#"));

    public static bool IsTest = false;
    
    public static IEnumerable<T> Log<T>(this IEnumerable<T> item, string message)
    {
        if (!IsTest)
            return item;
        
        return item.Select(i =>
        {
            Console.WriteLine($"{message}\t {i}");
            return i;
        });
    }

    public static TOut Map<TIn, TOut>(this TIn input, Func<TIn, TOut> func) => func(input);
}