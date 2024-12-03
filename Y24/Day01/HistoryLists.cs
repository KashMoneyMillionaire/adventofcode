using Core;

namespace Y24.Day01;

[DayOf(01)]
public class HistoryLists : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.EachLine()
                    .Split(' ')
                    .Parse<int>()
                    .Pairwise()
                    .Sorted()
                    .Select((x, y) => Math.Abs(x - y))
                    .Sum();
    }

    public static object SolvePart2(string input)
    {
        var (left, right) = input.EachLine()
                                 .Split(' ')
                                 .Parse<int>()
                                 .Pairwise()
                                 .Sorted()
                                 .BreakApart();
        
        var leftCounts = left.GroupCount();
        var rightCounts = right.GroupCount();

        return leftCounts.Select(l => l.Key * rightCounts.GetValueOrDefault(l.Key) * l.Value)
                         .Sum();
    }
}
