using Y22.Extensions;

namespace Y22.Day06;

[DayOf(06)]
public class SignalFinding : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return FindLastOfWindow(input, 4);
    }
    public static object SolvePart2(string input)
    {
        return FindLastOfWindow(input, 14);
    }

    private static object FindLastOfWindow(string input, int windowSize)
    {
        return input.ToArray()
                    .Window(windowSize)
                    .FirstOrDefault((chars, _) => chars.IsDistinct())
                    .Map(tup => tup.Index + windowSize);
    }
}
