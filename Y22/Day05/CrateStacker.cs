using Y22.Extensions;

namespace Y22.Day05;

[DayOf(05)]
public class CrateStacker : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines(skipEmptyLines: false)
                    .Map(Parse)
                    .Map(IterateInstructions1)
                    .Map(GetTopBoxes);
    }

    public static object SolvePart2(string input)
    {
        return input.ReadLines(skipEmptyLines: false)
                    .Map(Parse)
                    .Map(IterateInstructions2)
                    .Map(GetTopBoxes);
    }

    private static (List<Stack<char>> crates, IEnumerable<(int, int, int)> instructions) Parse(IEnumerable<string> input)
    {
        var split = input.Split(string.Empty)
                         .ToList();
        if (split is not [var crateInput, var instructionInput, ..])
            throw new("Bad input");

        var crates = crateInput.SkipLast(1)
                               .Aggregate(new List<IEnumerable<char>>(), ParseCrate)
                               .Map(ReverseAndEnqueue);

        var instructions = instructionInput.Select(l => l.RegexParse<int, int, int>(@"move (\d+) from (\d+) to (\d+)"));

        return (crates, instructions);
    }

    private static List<Stack<char>> ReverseAndEnqueue(List<IEnumerable<char>> queues)
    {
        return queues.Select(q => q.Reverse().Stack()).ToList();
    }

    private static List<IEnumerable<char>> ParseCrate(List<IEnumerable<char>> queues, string line)
    {
        var numOfCrates = (line.Length + 1) / 4;
        for (int i = 0; i < numOfCrates; i++)
        {
            if (queues.Count == i)
            {
                queues.Add(new List<char>());
            }

            if (line[i * 4] == ' ')
                continue;

            queues[i] = queues[i].Append(line[i * 4 + 1]);
        }

        return queues;
    }

    private static List<Stack<char>> IterateInstructions1((List<Stack<char>> crates, IEnumerable<(int move, int from, int onTo)> instructions) arg)
    {
        foreach (var instruction in arg.instructions)
        {
            for (int i = 0; i < instruction.move; i++)
            {
                arg.crates[instruction.onTo - 1].Push(arg.crates[instruction.from - 1].Pop());
            }
        }

        return arg.crates;
    }

    private static List<Stack<char>> IterateInstructions2((List<Stack<char>> crates, IEnumerable<(int move, int from, int onTo)> instructions) arg)
    {
        foreach (var instruction in arg.instructions)
        {
            var moved = Enumerable.Empty<char>();
            for (int i = 0; i < instruction.move; i++)
            {
                moved = moved.Append(arg.crates[instruction.from - 1].Pop());
            }

            foreach (char boxToMove in moved.Reverse())
            {
                arg.crates[instruction.onTo - 1].Push(boxToMove);
            }
        }

        return arg.crates;
    }
    
    private static string GetTopBoxes(List<Stack<char>> crates)
    {
        return crates.Select(c => c.Peek())
                     .Join("");
    }
}
