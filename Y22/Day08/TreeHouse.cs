using Y22.Extensions;

namespace Y22.Day08;

[DayOf(08)]
public class TreeHouse : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.AsMatrix<Tree>(Parse)
                    .Map(CountVisible);
    }

    public static object SolvePart2(string input)
    {
        var forest = input.AsMatrix<Tree>(Parse);
        return forest.Max(r => r.Max(c => c.Log(i => i.ToString(), true).ScenicScore).Log(_ => Environment.NewLine, true));
    }

    private static int CountVisible(Tree[][] forest)
    {
        return forest.Sum(r => r.Count(c => c.IsVisible));
    }

    private static Tree Parse(string tree, int x, int y, Tree[][] forest)
    {
        return new(tree.Parse<int>(), x, y, forest);
    }
}

public class Tree
{
    public Tree(int height, int x, int y, Tree[][] forest)
    {
        Height = height;

        N = y == 0 ? null : forest[x][y - 1];
        W = x == 0 ? null : forest[x - 1][y];

        if (N is not null) N.S = this;
        if (W is not null) W.E = this;
    }

    public int Height { get; }

    public Tree? N { get; private set; }
    public Tree? S { get; private set; }
    public Tree? E { get; private set; }
    public Tree? W { get; private set; }

    public int MaxN => N is null ? Height : Math.Max(N.MaxN, Height);
    public int MaxS => S is null ? Height : Math.Max(S.MaxS, Height);
    public int MaxE => E is null ? Height : Math.Max(E.MaxE, Height);
    public int MaxW => W is null ? Height : Math.Max(W.MaxW, Height);

    public bool VisibleN => N is null || N.MaxN < Height;
    public bool VisibleS => S is null || S.MaxS < Height;
    public bool VisibleE => E is null || E.MaxE < Height;
    public bool VisibleW => W is null || W.MaxW < Height;

    public bool IsVisible => VisibleN || VisibleS || VisibleE || VisibleW;

    public int ViewN => Score(Height, this, t => t.N);
    public int ViewS => Score(Height, this, t => t.S);
    public int ViewE => Score(Height, this, t => t.E);
    public int ViewW => Score(Height, this, t => t.W);

    private static int Score(int currHeight, Tree @this, Func<Tree, Tree?> getNext)
    {
        var next = getNext(@this);
        if (next == null)
            return 0;

        if (next.Height >= currHeight)
            return 1;

        return 1 + Score(currHeight, next, getNext);
    }
    
    public int ScenicScore => ViewN * ViewS * ViewE * ViewW;

    public override string ToString()
    {
        return $"Height: {Height} - {ScenicScore} - {ViewN} | {ViewS} | {ViewE} | {ViewW}";
    }
}