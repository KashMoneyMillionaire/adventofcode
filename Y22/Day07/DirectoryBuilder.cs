using System.Text;
using Y22.Extensions;

namespace Y22.Day07;

[DayOf(07)]
public class DirectoryBuilder : IDaySolver
{
    private static Directory GetRoot(string input)
        => input.ReadLines()
                .Select(Parse)
                .Aggregate(new Directory("/"), (dir, line) => line.Op(dir))
                .DoUntil(dir => dir.Parent!, dir => dir.Parent == null)
    // .Log(d => d.Debug(0), true)
    ;

    public static object SolvePart1(string input)
    {
        return GetRoot(input)
              .Map(r => r.Sizes)
              .Where(d => d.Size <= 100_000)
              .Sum(x => x.Size);
    }

    public static object SolvePart2(string input)
    {
        var root = GetRoot(input);

        const int TOTAL_DISK_SPACE = 70_000_000;
        const int NEEDED_SPACE = 30_000_000;
        var currentUsedSpace = root.Size;
        var unusedSpace = TOTAL_DISK_SPACE - currentUsedSpace;
        var spaceToDelete = NEEDED_SPACE - unusedSpace;

        return root.Sizes
                   .OrderBy(i => i.Size)
                   .First(i => i.Size > spaceToDelete)
                   .Map(i => i.Size);
    }

    private static CmdInput Parse(string line)
    {
        return line.Parse<CmdInput>();
    }
}

internal class Directory
{
    public Directory(string name)
    {
        Name = name;
    }

    public Directory(string name, Directory parent)
    {
        Name = name;
        Parent = parent;
    }

    public string Name { get; }
    public Directory? Parent { get; }

    public List<Directory> Dirs { get; } = new();
    public List<File> Files { get; } = new();

    public long Size 
        => Dirs.Sum(d => d.Size) + Files.Sum(f => f.Size);

    public IEnumerable<(Directory Dir, long Size)> Sizes
        => Dirs.SelectMany(d => d.Sizes).Append((this, Size));

    public object Debug(int depth)
    {
        var baseTabs = Enumerable.Repeat(' ', depth * 2).Join("");
        var subTabs = $"{baseTabs}  ";

        var current = $"/{Name} (Dir)";
        var sb = new StringBuilder($"{baseTabs}{current}\n");

        if (Dirs.Any())
        {
            var subDirs = Dirs.Select(d => d.Debug(depth + 1)).Join($"\n{subTabs}");
            sb.AppendLine($"{subTabs}{subDirs}");
        }

        var files = Files.Select(d => $"{d.Name} (file, size={d.Size})").Join($"\n{subTabs}");
        sb.Append($"{subTabs}{files}");

        return sb.ToString();
    }
    
    public override string ToString()
    {
        return $"{Name} - {Size}";
    }
}
