using Core;

internal record PuzzleDay(int Year, int Day, int Part)
{

    public PuzzleDay(DateTime time) : this(time.ToString("yy.dd.1")) {}
    public PuzzleDay(string input) : this(input.Split('.')) {}
    public PuzzleDay(string[] dayParts) : this(
        dayParts.NthOr(0, DateTime.Now.Year.ToString()).Parse<int>(),
        dayParts.NthOr(1, DateTime.Now.Date.ToString()).Parse<int>(),
        dayParts.NthOr(2, "1").Parse<int>()
    ) { }

    public string DayPart => Day.ToString("00");
    public string YearPart => Year.ToString()[^2..];
    
    public static PuzzleDay Parse(List<string> input, bool genNewDay)
    {
        var now = DateTime.Now;
        if (genNewDay)
        {
            return input switch
            {
                [] => new PuzzleDay(now),
                [var yearDay] when yearDay.Count('.') == 1 => new PuzzleDay($"{yearDay}.1"),
                [var day] => new PuzzleDay($"{now.Year}.{day}.1"),
                [var year, var day] => new PuzzleDay($"{year}.{day}.1"),
                    _ => throw new("PuzzleDay parsing error"),
            };
        }

        return input switch
        {
            [] => new PuzzleDay(now),
            [var part] when part.StartsWith('.') => new PuzzleDay($"{now.Year}.{now.Day}{part}"),
            [var dayPart] when dayPart.Count('.') == 1 => new PuzzleDay($"{now.Year}.{dayPart}"),
            [var yearDayPart] when yearDayPart.Count('.') == 2 => new PuzzleDay(yearDayPart),
            [var day] => new PuzzleDay($"{now.Year}.{day}.1"),
            _ => throw new("PuzzleDay parsing error"),
        };
    }

    internal string BuildPath()
    {
        string appDir = Environment.CurrentDirectory.DebugMe();
        string root = Path.Combine(appDir, "..")!.DebugMe();
        string path = $"{root}/Y{YearPart}/Day{DayPart}".DebugMe();

        return path;
    }
};
