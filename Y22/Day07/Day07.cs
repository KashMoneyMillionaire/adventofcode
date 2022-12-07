using Y22.Extensions;

namespace Y22.Day07;

[DayOf(07)]
public class Day07 : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .Select(Parse)
                    .Aggregate(new Directory("", null!), (dir, line) => dir.Op(line))
                    .DoUntil(dir => dir.Parent!, dir => dir.Parent == null).Log(d => d.Debug())
                    .Map(d => d.Sizes)
                    .Where(d => d.Size <= 100_000)
                    .Sum(x => x.Size);
    }

    public static object SolvePart2(string input)
    {
        throw new NotImplementedException();
    }

    private static Line Parse(string line)
    {
        return line switch
        {
            ['$', ' ', 'c', 'd', ' ', ..var path] => new Cd(path),
            "$ ls" => new Ls(),
            ['d', 'i', 'r', ' ', ..var dir] => new Dir(dir),
            _ => new File(line),
        };
    }
}

internal class Ls : Line
{
}

internal class Dir : Line
{
    public string Path { get; }

    public Dir(string path)
    {
        Path = path;
    }
}

internal class Cd : Line
{
    public string Path { get; }

    public Cd(string path)
    {
        Path = path;
    }
}

internal class Line
{
}

internal class Directory
{
    public Directory(string name, Directory parent)
    {
        Name = name;
        Parent = parent;
    }

    public override string ToString()
    {
        return $"{Name} - {Size}";
    }

    public string Name { get; }
    public Directory? Parent { get; }

    public List<Directory> Dirs { get; } = new();
    public List<File> Files { get; } = new ();

    public long Size => Dirs.Sum(d => d.Size) + Files.Sum(f => f.Size);

    public IEnumerable<(Directory Dir, long Size)> Sizes
        => Dirs.SelectMany(d => d.Sizes).Append((this, Size));

    public List<(Directory Dir, long Size)> SizesList => Sizes.ToList();

    public Directory Move(string path)
    {
        return path switch
        {
            ".." => Parent!,
            "/" => this,
            _ => Dirs.First(d => d.Name == path)
        };
    }

    public Directory Add(Dir dir)
    {
        if (Dirs.All(d => d.Name != dir.Path))
            Dirs.Add(new(dir.Path, this));

        return this;
    }

    public Directory Add(File file)
    {
        if (Files.All(f => f.Name != file.Name))
            Files.Add(file);

        return this;
    }

    public Directory Op(Line line)
    {
        return line switch
        {
            Cd cd => Move(cd.Path),
            Ls => this,
            Dir dir => Add(dir),
            File file => Add(file)
        };
    }

    public object Debug()
    {
        return $"""
        /{Name} - {Size}
            {Dirs.Select(d => d.Debug()).Join("\n\t")}
            {Files.Select(d => d.Name).Join("\n\t")}
        """;
    }
}

internal class File : Line
{
    public string Name { get; }
    public long Size { get; }

    public File(string file)
    {
        var items = file.Split(" ");
        Size = items[0].Parse<long>();
        Name = items[1];
    }
}