
internal static class DayGenerator
{
    public static void Generate(PuzzleDay puzzleDay)
    {
        string fileContents = TEMPLATE.Replace("{{dayNumber}}", puzzleDay.DayPart)
                                      .Replace("{{yearNumber}}", puzzleDay.YearPart);

        var path = puzzleDay.BuildPath();
        if (Directory.Exists(path)) 
        {
            Console.WriteLine("Day already generated, skipping");
            return;
        }

        Console.WriteLine($"Generating files for: {path}");
        Directory.CreateDirectory(path);
        File.WriteAllText($"{path}/test.txt", "");
        File.WriteAllText($"{path}/input.txt", "");
        File.WriteAllText($"{path}/Temp.cs", fileContents);
    }

    private const string TEMPLATE = """
    namespace Y{{yearNumber}}.Day{{dayNumber}};

    [DayOf({{dayNumber}})]
    public class Day{{dayNumber}} : IDaySolver
    {
        public static object SolvePart1(string input)
        {
            throw new NotImplementedException();
        }

        public static object SolvePart2(string input)
        {
            throw new NotImplementedException();
        }
    }
    
    """;
}
