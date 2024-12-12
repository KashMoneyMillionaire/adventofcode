namespace Core;

public class Input
{
    private readonly string _initialValue;

    public List<string> Lines { get; }
    public string Cleaned { get; }

    public Input(
        string value,
        bool skipEmptyLines = true,
        string? splitOn = null,
        int take = int.MaxValue)
    {
        _initialValue = value;

        Lines = value.ReplaceLineEndings()
                     .Split(splitOn ?? Environment.NewLine)
                     .Where(l => !skipEmptyLines || !string.IsNullOrWhiteSpace(l))
                     .Where(l => !l.StartsWith('#'))
                     .Take(take)
                     .ToList();

        Cleaned = Lines.Join(Environment.NewLine);
    }

    public static implicit operator string(Input input)
    {
        return input.Cleaned;
    }
}

public static partial class InputExtensions
{
    public static Input AsInput(this string val) => new(val);

    public static string ParseInput(this string val) => new Input(val).Cleaned;
}