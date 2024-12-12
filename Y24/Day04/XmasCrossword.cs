using Core;

namespace Y24.Day04;

[DayOf(04)]
public class XmasCrossword : IDaySolver
{
    public static object SolvePart1(string input)
    {
        var matrix = input.AsInput()
                          .ToMatrix();

        return matrix.Search("XMAS", SearchDirections.All)
                     .DebugUs()
                     .Count()
                    //  .DebugMe()
                     ;
    }

    public static object SolvePart2(string input)
    {
        var matrix = input.AsInput()
                          .ToMatrix();

        char[][] search = [
            ['M', '.', 'S'],
            ['.', 'A', '.'],
            ['M', '.', 'S']
        ];

        return matrix.Search(search)
                     .DebugUs()
                     .Count()
                    //  .DebugMe()
                     ;
    }
}
