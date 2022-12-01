namespace Year2021;

public static class Extensions
{
    public static long? MinNotNull<TSource>(this IEnumerable<TSource?> items, Func<TSource?, long?> func)
    {
        return items.Select(func)
                    .Where(i => i is not null)
                    .Min();
    }

    public static void Print(this Space[,] spaces, List<Amphipod> before, List<Amphipod> after)
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 13; x++)
            {
                var space = spaces[x, y];
                var filledWith = space.FilledWith(before);
                var c = space.IsWall ? '#' : space.IsSpace ? ' ' : filledWith?.Type ?? '.';
                Console.Write(c);
            }

            Console.Write(y == 2 ? " -> " : "    ");

            for (int x = 0; x < 13; x++)
            {
                var space = spaces[x, y];
                var filledWith = space.FilledWith(after);
                var c = space.IsWall ? '#' : space.IsSpace ? ' ' : filledWith?.Type ?? '.';
                Console.Write(c);
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine();
    }

    public static int Opposite(this int x, int a, int b)
    {
        return x == a ? b : a;
    }

    public static bool OneOf<T>(this T x, params T[] values)
    {
        return values.Contains(x);
    }
    
    public static bool IsHallway(this (int X, int Y) coordinates)
    {
        (int x, int y) = coordinates;
        return y == 1 && x is > 0 and < 12;
    }

    public static bool IsBetween(this int v, int a, int b)
    {
        return Math.Min(a, b) < v && v < Math.Max(a, b);
    }
}