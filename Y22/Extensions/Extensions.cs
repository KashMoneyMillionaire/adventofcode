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
        => input.ReplaceLineEndings()
                .Split(splitOn ?? Environment.NewLine)
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

    public static T Log<T>(this T item, Func<T, string> map)
    {
        if (!IsTest)
            return item;

        Console.WriteLine(map(item));
        return item;
    }

    public static T Log<T>(this T item, Func<T, object> func, bool overrideTest = false, string? message = null)
    {
        if (!IsTest && !overrideTest)
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

    public static Queue<T> AsQueue<T>(this IEnumerable<T> items) => new(items);

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
        var alreadySeen = new HashSet<T>();
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
        while (true)
        {
            if (until(item)) return item;
            item = doThis(item);
        }
    }

    public static T DoTimes<T>(this T item, Func<T, T> doThis, int numTimes)
    {
        while (true)
        {
            if (numTimes-- <= 0) return item;
            item = doThis(item);
        }
    }

    public static T[][] AsMatrix<T>(this string input, Func<string, int, int, T[][], T> transform)
    {
        var rows = input.ReadLines().ToList();
        var enumerable = rows.SelectMany((l, y) => l.Select((c, x) => (c, x, y)));

        var forest = new T[rows.Count][];
        foreach (var cell in enumerable)
        {
            forest[cell.x] ??= new T[rows[0].Length];
            forest[cell.x][cell.y] = transform(cell.c.ToString(), cell.x, cell.y, forest);
        }

        return forest;
    }

    public static bool IsAdjacent(this (int x, int y) head, (int x, int y) tail)
    {
        return Math.Abs(head.x - tail.x) <= 1 && Math.Abs(head.y - tail.y) <= 1;
    }

    public static IEnumerable<TState> Select<T, TState>(this IEnumerable<T> items, TState state,
                                                        Func<TState, T, TState> func)
    {
        foreach (var item in items)
        {
            state = func(state, item);
            yield return state;
        }
    }

    public static IEnumerable<T1> SelectFirst<T1, T2>(this IEnumerable<(T1, T2)> items)
        => items.Select(i => i.Item1);

    public static bool IsBetween(this int val, int left, int right)
        => left <= val && val <= right;

    public static bool IsWithin(this int val, int baseVal, int offsetLeft, int offsetRight)
        => val.IsBetween(baseVal + offsetLeft, baseVal + offsetRight);

    public static IEnumerable<T> Stream<T>(this T item)
    {
        while (true)
        {
            yield return item;
        }
    }

    public static IEnumerable<T> Stream<T>(this T item, Action<T> toDo)
        => item.Stream().Select(i =>
        {
            toDo(i);
            return i;
        });

    public static IEnumerable<T> Stream<T>(this T item, Action<T> toDo, Func<T, bool> doUntil)
        => item.Stream()
               .Select(i =>
                {
                    toDo(i);
                    return i;
                })
               .TakeWhile(i => !doUntil(i));

    public static (T, T) Take2<T>(this IEnumerable<T> items)
    {
        return (items.First(), items.Skip(1).First());
    }
}