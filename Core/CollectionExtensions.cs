namespace Core;

public static class CollectionExtensions
{
    public static Dictionary<T, int> GroupCount<T>(this IEnumerable<T> source) where T : notnull
    {
        return source.GroupBy(x => x)
                     .ToDictionary(x => x.Key, x => x.Count());
    }
}