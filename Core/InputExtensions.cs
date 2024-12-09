using System.Text.RegularExpressions;

namespace Core;

public static class InputExtensions 
{
    public static bool IsDebug => Extensions.IsDebug;

    public static IEnumerable<string> EachLine(this string input, int? row = null, int? skip = null, int? take = null) 
    {
        var split = input.Split('\n');

        if (row is {} idx)
            return split[idx-1].AsList();

        return split.Skip(skip ?? 0).TakeIfNotNull(take ?? split.Length);
    }

    public static IEnumerable<string[]> Split(this IEnumerable<string> source, char splitOn) 
    {
        return source.Select(s => s.Split(splitOn, StringSplitOptions.RemoveEmptyEntries).DebugUs().ToArray());
    }

    public static IEnumerable<TTo> Parse<TTo>(this IEnumerable<string> source) where TTo : IParsable<TTo>
    {
        return source.Select(s => s.Parse<TTo>());
    }

    public static IEnumerable<TTo> Parse<TTo>(this MatchCollection source) where TTo : IRegexParsable<TTo>
    {
        return source.Select(TTo.Parse);
    }

    public static IEnumerable<TTo[]> Parse<TTo>(this IEnumerable<string[]> source) where TTo : IParsable<TTo>
    {
        return source.Select(s => s.Parse<TTo>().ToArray());
    }

    public static IEnumerable<(T X, T Y)> Zip2<T>(this IEnumerable<T[]> source)
    {
        return source.Select(s => (s[0], s[1]));
    }

    public static IEnumerable<(T X, T Y)> Sorted<T>(this IEnumerable<(T X, T Y)> source) 
    {
        var left = source.Select(s => s.X);
        var right = source.Select(s => s.Y);

        return left.Order().Zip(right.Order());
    }

    public static IEnumerable<T> Select<T>(this IEnumerable<(T X, T Y)> source, Func<T, T, T> selector)
    {
        return source.Select(s => selector(s.X, s.Y));
    }
    public static (IEnumerable<T> X, IEnumerable<T> Y) BreakApart<T>(this IEnumerable<(T X, T Y)> source)
    {
        return (source.Select(s => s.X), source.Select(s => s.Y));
    }
}