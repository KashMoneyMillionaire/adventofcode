using System.Reflection;
using Y22;
using Y22.Helpers;

while (true) Run();

void Run()
{
    Console.Write("Input: ");
    string input = Console.ReadLine()!;

    Extensions.IsTest = input.EndsWith(".");
    
    string result = input switch
    {
        "" => throw new("ending"),
        ['.', '.', ' ', ..var day] => DayGenerator.Generate(int.Parse(day)),
        [..var day, '.', var part] => Solve(int.Parse(day), part, GetInput(int.Parse(day), "input.txt")),
        [..var day, '.', var part, '.'] => Solve(int.Parse(day), part, GetInput(int.Parse(day), "test.txt")),
        _ => "Unknown"
    };

    result.Print("The solution is: ");
}


string Solve(int day, char part, string input)
{
    var solver = typeof(IDaySolver).Assembly
                                   .GetTypes()
                                   .Where(t => t.IsAssignableTo(typeof(IDaySolver))
                                               && !t.IsInterface)
                                   .Single(t => t.GetCustomAttribute<DayOfAttribute>()?.Day == day);

    try
    {
        return solver.GetMethod($"SolvePart{part}")?.Invoke(null, new object[] { input })?.ToString()
            ?? throw new("Couldn't find method");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return "Error";
    }
}

string GetInput(int day, string fileName)
{
    return File.ReadAllText($"Day{day:00}/{fileName}");
}

public interface IDaySolver
{
    static abstract object SolvePart1(string input);
    static abstract object SolvePart2(string input);
}

public class DayOfAttribute : Attribute
{
    public int Day { get; set; }

    public DayOfAttribute(int dayNumber)
    {
        Day = dayNumber;
    }
}