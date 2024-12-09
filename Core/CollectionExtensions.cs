namespace Core;

public static class CollectionExtensions
{
    public static Dictionary<T, int> GroupCount<T>(this IEnumerable<T> source) where T : notnull
    {
        return source.GroupBy(x => x)
                     .ToDictionary(x => x.Key, x => x.Count());
    }

    public static IEnumerable<(T Prev, T Curr)> Pairwise<T>(this IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
            yield break;

        T previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }
    
    public static IEnumerable<(T Prev, T Curr)> Pairwise<T>(this IEnumerable<T> source, Func<T, T, bool>? filter = null)
    {
        filter ??= (_, _) => true;

        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
            yield break;

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (filter(previous, current)) 
            {
                yield return (previous, enumerator.Current);
                previous = enumerator.Current;
            }
        }
    }

    public static IEnumerable<T> TakeIfNotNull<T>(this IEnumerable<T> source, int? takeNull)
    {
        return (takeNull is {} take) ? source.Take(take) : source;
    }

    public static IEnumerable<T> WithoutIndex<T>(this IEnumerable<T> source, params int[] idxs)
    {
        return source.Where((x, i) => !idxs.Contains(i));
    }

    public static IEnumerable<(T Item, int Idx)> Iterate<T>(this T source, int from, int to)
    {
        return Enumerable.Range(from, to).Select(i => (source, i));
    }
    public static bool IsOrdered<T>(this IEnumerable<T> source)
    {
        return source.SequenceEqual(source.Order()) || source.SequenceEqual(source.Order().Reverse());
    }

    public static List<T> AsList<T>(this T value) => [value];
}