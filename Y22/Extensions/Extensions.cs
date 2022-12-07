﻿namespace Y22.Extensions;

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
                .Where(l => !skipEmptyLines || !string.IsNullOrWhiteSpace(l))
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

    public static T Log<T>(this T item, string? message = null)
    {
        if (!IsTest)
            return item;
        
        Console.WriteLine($"{message}{item}");
        return item;
    }

    public static T Log<T>(this T item, Func<T, object> func, string? message = null)
    {
        if (!IsTest)
            return item;
        
        Console.WriteLine($"{message}{func(item)}");
        return item;
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

    public static Queue<T> Queue<T>(this IEnumerable<T> items) => new(items);

    public static Stack<T> Stack<T>(this IEnumerable<T> items) => new(items);

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> input, T splitOn)
        where T : IEquatable<T>
    {
        var currentSet = Enumerable.Empty<T>();
        foreach (var item in input)
        {
            if (item.Equals(splitOn))
            {
                yield return currentSet;
                currentSet = Enumerable.Empty<T>();
                continue;
            }

            currentSet = currentSet.Append(item);
        }

        yield return currentSet;
    }

    public static string Join<T>(this IEnumerable<T> items, string joinWith)
        => string.Join(joinWith, items);

    public static bool IsDistinct<T>(this IEnumerable<T> input)
    {
        var alreadySeen=new HashSet<T>();
        return input.All(item => alreadySeen.Add(item));
    }

    public static IEnumerable<IEnumerable<T>> Window<T>(this T[] items, int windowSize)
    {
        for (int i = 0; i < items.Length - windowSize; i++)
        {
            yield return items[i..(i + windowSize)];
        }
    }

    public static (T Item, int Index) FirstOrDefault<T>(this IEnumerable<T> items, Func<T, int, bool> func)
    {
        return items.Select((item, i) => (item, i))
                    .FirstOrDefault(tuple => func(tuple.item, tuple.i));
    }

    public static T DoUntil<T>(this T item, Func<T, T> doThis, Func<T, bool> until)
    {
        return until(item) ? item : doThis(item);
    }
}