namespace Y22.Extensions;

public static class Extensions
{
    public static void Print(this string value, string prefix) => Console.WriteLine($"{prefix}{value}");

    public static IEnumerable<string> ReadLines(
        this string input, 
        bool skipEmptyLines = true, 
        string? splitOn = null,
        int take = int.MaxValue
        )
        => input.Split(splitOn ?? Environment.NewLine)
                .Where(l => skipEmptyLines && !string.IsNullOrWhiteSpace(l))
                .Where(l => !l.StartsWith("#"))
                .Take(take);

    public static bool IsTest = false;
    
    public static IEnumerable<T> Log<T>(this IEnumerable<T> item, string? message = null)
    {
        if (!IsTest)
            return item;
        
        return item.Select(i =>
        {
            Console.WriteLine($"{message}{i}");
            return i;
        });
    }

    public static TOut Map<TIn, TOut>(this TIn input, Func<TIn, TOut> func) => func(input);

    public static T Parse<T>(this string input) where T : IParsable<T>
    {
        return T.Parse(input, null);
    }
    
    public static bool Covers(this Range r1, Range r2) 
        => r1.Start.Value <= r2.Start.Value && r1.End.Value >= r2.End.Value;
    
    public static bool Overlaps(this Range r1, Range r2)
    {
        bool b = r1.End.Value >= r2.Start.Value && r1.Start.Value <= r2.End.Value;
        bool overlaps1 = r1.Start.Value <= r2.End.Value && r1.End.Value >= r2.Start.Value;
        return b || overlaps1;
    }
}