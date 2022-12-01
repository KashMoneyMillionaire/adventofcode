namespace Y22.Day01;

[DayOf(1)]
public class Calories : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}")
                    .Select(l => l.Split(Environment.NewLine)
                                  .Select(int.Parse)
                                  .Sum())
                    .Max();
    }

    public static object SolvePart2(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}")
                    .Select(l => l.Split(Environment.NewLine)
                                  .Select(int.Parse)
                                  .Sum())
                    .OrderDescending()
                    .Take(3)
                    .Sum();
    }
}