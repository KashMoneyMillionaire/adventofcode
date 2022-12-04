using Y22.Extensions;

namespace Y22.Day04;

[DayOf(04)]
public class OverlappingCleanup : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .Select(Parse).Log()
                    .Where(EitherCovers)
                    .Count();
    }

    public static object SolvePart2(string input)
    {
        return input.ReadLines()
                    .Select(Parse).Log()
                    .Where(AnyOverlap)
                    .Count();
    }

    private static bool EitherCovers((Range, Range) arg)
    {
        return arg.Item1.Covers(arg.Item2) || arg.Item2.Covers(arg.Item1);
    }

    private static bool AnyOverlap((Range, Range) arg)
    {
        return arg.Item1.Overlaps(arg.Item2);
    }

    private static (Range, Range) Parse(string line)
    {
        var (l1, l2, r1, r2) = line.RegexParse<int, int, int, int>(@"(\d+)-(\d+),(\d+)-(\d+)");
        return (new(l1, l2), new(r1, r2));
    }
}
