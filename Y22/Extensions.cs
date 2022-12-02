namespace Y22;

public static class Extensions
{
    public static void Print(this string value, string prefix) => Console.WriteLine($"{prefix}{value}");

    public static IEnumerable<string> ReadLines(this string input, bool skipEmptyLines = true, string? splitOn = null)
        => input.Split(splitOn ?? Environment.NewLine)
                .Where(l => skipEmptyLines && !string.IsNullOrWhiteSpace(l))
                .Where(l => !l.StartsWith("#"));
}
