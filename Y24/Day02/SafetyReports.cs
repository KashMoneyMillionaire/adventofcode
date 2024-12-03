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
                          .Where(r => r.IsSafe)
                          .Count();

        return answer;
    }

    public static object SolvePart2(string input)
    {
        var answer = input.EachLine()
                          .Parse<Report>()
                          .Where(r => r.IsSafe)
                          .Count();

        return answer;
    }
}

public record Report(List<int> Levels) : IParsable<Report>
{
    public bool IsSafe => IsGraduallyDecreasing || IsGraduallyIncreasing;

    public bool IsGraduallyIncreasing 
        => Levels.Pairwise()
                 .All(i => i.DebugMe().Prev < i.Curr && Math.Abs(i.Prev - i.Curr).IsBetween(1, 3));

    public bool IsGraduallyDecreasing 
        => Levels.Pairwise()
                 .All(i => i.Prev > i.Curr && Math.Abs(i.Prev - i.Curr).IsBetween(1, 3));

    public static Report Parse(string s, IFormatProvider? provider)
    {
        return new Report(s.Split(' ').Parse<int>().ToList());
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Report result)
    {
        throw new NotImplementedException();
    }
}