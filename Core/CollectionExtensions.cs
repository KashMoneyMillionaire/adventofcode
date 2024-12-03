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
}