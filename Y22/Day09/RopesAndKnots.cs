using Y22.Extensions;
using static Y22.Extensions.ParsingExtensions;

namespace Y22.Day09;

[DayOf(09)]
public class RopesAndKnots : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .Select(l => l.RegexParse<Direction, int>(@"(\D) (\d+)"))
                    .Aggregate((new List<(int X, int Y)> { (0, 0) }, (0, 0), (0, 0)), Walk)
                    .Map(r => r.Item1)
                    .Distinct()
                    .Count();
    }

    private static (List<(int X, int Y)> Trail, (int, int), (int, int)) Walk((List<(int X, int Y)> Trail, (int x, int y) H, (int x, int y) T) state, (Direction D, int Steps) input)
    {
        (int hx, int hy) = state.H;
        (int tx, int ty) = state.T;
        
        for (int i = 0; i < input.Steps; i++)
        {
            switch (input.D.D)
            {
                case "R":
                    hx++;
                    (tx, ty) = Move(hx, hy, tx, ty);
                    break;
                case "L":
                    hx--;
                    (tx, ty) = Move(hx, hy, tx, ty);
                    break;
                case "U":
                    hy++;
                    (tx, ty) = Move(hx, hy, tx, ty);
                    break;
                case "D":
                    hy--;
                    (tx, ty) = Move(hx, hy, tx, ty);
                    break;
            }
            
            state.Trail.Add((tx, ty));
        }

        return (state.Trail, (hx, hy), (tx, ty));
    }

    private static (int tx, int ty) Move(int hx, int hy, int tx, int ty)
    {
        if ((hx, hy).IsAdjacent((tx, ty)))
            return (tx, ty);
        
        // Head is above tail
        if (hx == tx && hy > ty)
            return (hx, hy - 1);
        
        // Head is below tail
        if (hx == tx && hy < ty)
            return (hx, hy + 1);
        
        // Head is left of tail
        if (hy == ty && hx < tx)
            return (hx + 1, hy);
        
        // Head is right of tail
        if (hy == ty && hx > tx)
            return (hx - 1, hy);
        
        // Head is above diagonally left
        if (hy > ty && hx < tx)
            return (tx - 1, ty + 1);
        
        // Head is above diagonally right
        if (hy > ty && hx > tx)
            return (tx + 1, ty + 1);
        
        // Head is below diagonally left
        if (hy < ty && hx < tx)
            return (tx - 1, ty - 1);
        
        // Head is below diagonally right
        if (hy < ty && hx > tx)
            return (tx + 1, ty - 1);
        
        throw new("Shouldn't get here");
    }

    public static object SolvePart2(string input)
    {
        throw new NotImplementedException();
    }
}

public class Direction : IParsable<Direction>
{
    public string D { get; set; }

    private Direction(string d)
    {
        D = d;
    }

    public static Direction Parse(string s, IFormatProvider? provider)
    {
        return new(s);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Direction result)
    {
        throw new NotImplementedException();
    }
    
    public static implicit operator string(Direction d) => d.D;
    public static implicit operator Direction(string s) => new(s);
}