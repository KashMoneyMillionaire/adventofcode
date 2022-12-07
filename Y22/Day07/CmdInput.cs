using Y22.Extensions;

namespace Y22.Day07;

internal class CmdInput : IParsable<CmdInput>
{
    public static CmdInput Parse(string input, IFormatProvider? provider)
    {
        if (TryParse(input, provider, out var line))
            return line;

        throw new("Bad input");
    }

    public static bool TryParse(string? input, IFormatProvider? provider, out CmdInput cmdInput)
    {
        cmdInput = input switch
        {
            ['$', ' ', 'c', 'd', ' ', ..var path] => new Cd(path),
            "$ ls" => new Ls(),
            ['d', 'i', 'r', ' ', ..var dir] => new Dir(dir),
            _ => new File(input!),
        };

        return true;
    }
    
    public Directory Op(Directory currDir)
    {
        return this switch
        {
            Cd cd => Move(currDir, cd.Path),
            Ls => currDir,
            Dir dir => Add(currDir, dir),
            File file => Add(currDir, file),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Directory Move(Directory currDir, string path)
    {
        return path switch
        {
            ".." => currDir.Parent!,
            "/" => currDir.Parent == null ? currDir : Move(currDir.Parent, "/"),
            _ => currDir.Dirs.First(d => d.Name == path)
        };
    }

    private static Directory Add(Directory currDir, Dir dir)
    {
        if (currDir.Dirs.All(d => d.Name != dir.Path))
            currDir.Dirs.Add(new(dir.Path, currDir));

        return currDir;
    }

    private static Directory Add(Directory currDir, File file)
    {
        if (currDir.Files.All(f => f.Name != file.Name))
            currDir.Files.Add(file);

        return currDir;
    }

}


internal class Ls : CmdInput
{
}

internal class Dir : CmdInput
{
    public string Path { get; }

    public Dir(string path)
    {
        Path = path;
    }

    public override string ToString()
    {
        return $"Dir {Path}";
    }
}

internal class Cd : CmdInput
{
    public string Path { get; }

    public Cd(string path)
    {
        Path = path;
    }

    public override string ToString()
    {
        return $"Cd {Path}";
    }
}

internal class File : CmdInput
{
    public string Name { get; }
    public long Size { get; }

    public File(string file)
    {
        var items = file.Split(" ");
        Size = items[0].Parse<long>();
        Name = items[1];
    }

    public override string ToString()
    {
        return $"File {Name} - {Size}";
    }
}