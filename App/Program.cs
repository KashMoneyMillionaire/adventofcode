using System.Reflection;
using Core;
using Y24.Day01;


Run();

void Run()
{
    var useTestInput = args.Contains("test");
    var genNewDay = args.Contains("gen");
    var shouldLoop = args.Contains("loop");
    Extensions.IsDebug = args.Contains("debug");

    var input = args.Except("test", "gen", "loop", "debug").ToList();

    var day = PuzzleDay.Parse(input, genNewDay);
    Console.WriteLine($"Found day: {day}");

    if (genNewDay)
    {
        DayGenerator.Generate(day);
        return;
    }

    do
    {
        var inputFile = useTestInput ? "test.txt" : "input.txt";
        var answer = Solve(day, GetInput(day, inputFile));

        $"The solution is: {answer}".Print();
    } while (shouldLoop);
}


string Solve(PuzzleDay puzzleDay, string input)
{
    $"Looking for assembly: Y{puzzleDay.YearPart}".Debug();
    var solver = 
        Assembly.Load($"Y{puzzleDay.YearPart}").DebugMe()
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IDaySolver)) && !t.IsInterface)
                .Single(t => t.GetCustomAttribute<DayOfAttribute>()?.Day == puzzleDay.Day);
    $"Found solver: {solver.Name}".Debug();

    try
    {
        return solver.GetMethod($"SolvePart{puzzleDay.Part}")?.Invoke(null, [input])?.ToString()
            ?? throw new("Couldn't find method");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return "Error";
    }
}

string GetInput(PuzzleDay day, string fileName)
{
    return File.ReadAllText(Path.Combine(day.BuildPath(), fileName));
}
