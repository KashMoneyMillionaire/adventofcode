namespace Y24.Day04;

[DayOf(04)]
public class XmasCrossword : IDaySolver
{
    public static object SolvePart1(string input)
    {
        var matrix = input.ParseInput().AsMatrix<int>();

        return matrix.Search("XMAS", )

        return input;
    }

    public static object SolvePart2(string input)
    {
        return input;
    }
}
