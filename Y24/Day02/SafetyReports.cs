using System.Diagnostics.CodeAnalysis;
using Core;

namespace Y24.Day02;

[DayOf(02)]
public class SafetyReports : IDaySolver
{
    public static object SolvePart1(string input)
    {
        var answer = input.EachLine()
                          .Parse<Report>()
                          .Where(r => r.IsSafe(0))
                          .Count();

        return answer;
    }

    public static object SolvePart2(string input)
    {
        var answer = input.EachLine(row: 100)
                          .Parse<Report>().DebugUs(r => r.Debug(1))
                          .Where(r => r.IsSafe(1))
                          .Count();

        return answer;
    }
}

public record Report(List<int> Levels) : IParsable<Report>
{
    public string Debug(int numMistakesAllowed) 
        => $"{ListsToAsses(numMistakesAllowed).Select(l => l.Join(", ")).Join("\n   ")}";

    public bool IsSafe(int numMistakesAllowed)
    {
        var lists = ListsToAsses(numMistakesAllowed);

        return lists.Any(l => l.IsOrdered()
                              && l.Pairwise().All((pair) => Math.Abs(pair.Prev - pair.Curr).IsBetween(1, 3)));
    }

    public List<List<int>> ListsToAsses(int numMistakesAllowed)
    {
        if (numMistakesAllowed == 0)
            return [Levels];

        if (numMistakesAllowed > 1)
            throw new NotImplementedException("🤷");

        var sizes = Levels.Count - numMistakesAllowed;
        return Levels.Iterate(0, Levels.Count)
                     .Select(x => x.Item.WithoutIndex(x.Idx).ToList())
                     .ToList();
    }

    public static Report Parse(string s, IFormatProvider? provider)
    {
        return new Report(s.Split(' ').Parse<int>().ToList());
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Report result)
    {
        throw new NotImplementedException();
    }
}