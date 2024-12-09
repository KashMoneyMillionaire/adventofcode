using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Y24.Day03;

[DayOf(03)]
public partial class Multiplier : IDaySolver
{
    [GeneratedRegex(@"(mul\((\d+),(\d+)\))")]
    public static partial Regex MulltiRegex { get; }


    public static object SolvePart1(string input)
    {
        return MulltiRegex.Matches(input)
                          .Parse<Mul>()
                          .Sum(m => m.Val);
    }

    public static object SolvePart2(string input)
    {
        throw new NotImplementedException();
    }
}

public record Mul(int X, int Y) : IRegexParsable<Mul>
{
    public int Val => X * Y;

    public static Mul Parse(Match match)
    {
        return new(match.Groups[2].Value.Parse<int>(), match.Groups[3].Value.Parse<int>());
    }
}
