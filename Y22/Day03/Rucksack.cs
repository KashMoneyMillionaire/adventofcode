using Y22.Extensions;

namespace Y22.Day03;

[DayOf(03)]
public class Rucksack : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .Select(SplitRucksack)
                    .Select(FindOverlap).Log(   "Overlap:")
                    .Select(ScoreFoundItem).Log("Score:   ")
                    .Sum();
    }
    
    public static object SolvePart2(string input)
    {
        return input.ReadLines()
                    .Map(SplitGroups)
                    .Select(FindOverlap)
                    .Select(ScoreFoundItem)
                    .Sum();
    }

    private static HashSet<char[]> SplitRucksack(string arg)
    {
        return arg.Chunk(arg.Length / 2)
                  .ToHashSet();
    }

    private static IEnumerable<HashSet<char[]>> SplitGroups(IEnumerable<string> lines)
    {
        return lines.Select(l => l.ToArray())
                    .Chunk(3)
                    .Select(l => l.ToHashSet());
    }

    private static char FindOverlap(HashSet<char[]> rucksack)
    {
        return rucksack.First()
                       .First(i => rucksack.All(c => c.Contains(i)));
    }

    private static int ScoreFoundItem(char item)
    {
        return char.IsUpper(item) ? item - 38 : item - 96;
    }
}
