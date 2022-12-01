namespace Y22.Helpers;

public static class DayGenerator
{
    public static string Generate(int dayNumber)
    {
        string day = $"{dayNumber:00}";
        string className = $"Day{day}";
        string fileContents = TEMPLATE.Replace("{{dayNumber}}", day)
                                      .Replace("{{className}}", className);

        string root = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        string path = $"{root}/Day{day}";
        Directory.CreateDirectory(path);
        File.WriteAllText($"{path}/test.txt", "");
        File.WriteAllText($"{path}/input.txt", "");
        File.WriteAllText($"{path}/{className}.cs", fileContents);

        return "New Day Generated";
    }

    private const string TEMPLATE = """
    namespace Y22.Day{{dayNumber}};

    [DayOf({{dayNumber}})]
    public class {{className}} : IDaySolver
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