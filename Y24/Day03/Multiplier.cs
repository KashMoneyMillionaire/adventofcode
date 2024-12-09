using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Y24.Day03;

[DayOf(03)]
public partial class Multiplier : IDaySolver
{

    [GeneratedRegex(@"(mul\((\d+),(\d+)\)|(do\(\)|don't\(\)))")]
    public static partial Regex MulltiRegex { get; }


    public static object SolvePart1(string input)
    {
        Tracker.Enabled = false;
        var computer = new Computer(MulltiRegex.Matches(input).Parse<Instruction>());
        return computer.Run();
    }

    public static object SolvePart2(string input)
    {
        Tracker.Enabled = true;
        var computer = new Computer(MulltiRegex.Matches(input).Parse<Instruction>());
        return computer.Run();
    }
}

public class Computer(IEnumerable<Instruction> Instructions)
{
    public int Run()
    {
        var agg = 0;
        foreach (var instruction in Instructions)
        {
            if (instruction is Tracker)
            {
                instruction.Do();
                continue;
            }

            if (Tracker.Enabled && !Tracker.TrackerOn)
                continue;

            agg += instruction switch
            {
                Mul m => m.DebugMe().Val,
                _ => throw new NotImplementedException()
            };
        }

        return agg;
    }
}

public abstract record Instruction : IRegexParsable<Instruction>
{
    public abstract void Do();

    public static Instruction Parse(Match match)
    {
        match.DebugMe();
        if (match.Value == "don't()") return new Tracker(false);
        if (match.Value == "do()") return new Tracker(true);

        return Mul.Parse(match);
    }
}

public record Tracker(bool SetTrackerTo) : Instruction
{
    public static bool Enabled { get; internal set; }
    public static bool TrackerOn { get; private set; } = true;

    public override void Do()
    {
        TrackerOn = SetTrackerTo;
    }
}

public record Mul(int X, int Y) : Instruction, IRegexParsable<Mul>
{
    public int Val => X * Y;

    public new static Mul Parse(Match match)
    {
        return new(match.Groups[2].Value.Parse<int>(), match.Groups[3].Value.Parse<int>());
    }

    public override void Do()
    {
        throw new NotImplementedException();
    }
}
