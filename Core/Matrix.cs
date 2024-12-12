
using System.Buffers;
using System.Linq;

namespace Core;

public static class MatrixExtensions
{
    public static IEnumerable<char[]> AsMatrix(this Input input)
    {
        return input.Lines.Select(l => l.ToCharArray());
    }

    public static IEnumerable<SearchResult> Search<T>(this IEnumerable<char[]> matrix, string searchTerm, SearchDirections directions)
    {
        // Horizontal
        List<string> horizontalSearch = [];
        if (directions.HasFlag(SearchDirections.LeftToRight))
            horizontalSearch.Add(searchTerm);
        if (directions.HasFlag(SearchDirections.RightToLeft))
            horizontalSearch.Add(new string(searchTerm.Reverse().ToArray()));
        if (horizontalSearch.Any())
        {
            foreach (var row in matrix)
            {
                ReadOnlySpan<char> x = row;
                var sv = SearchValues.Create(horizontalSearch);
                var z = x.IndexOfAny(SearchValues.Create(horizontalSearch));
            }
        }

        yield break;
    }

}

public record SearchResult(int X, int Y, SearchDirections Direction);

[Flags]
public enum SearchDirections
{
    Vertical = TopToBottom | BottomToTop,
    TopToBottom = 1,
    BottomToTop = 2,

    Horizontal = LeftToRight | RightToLeft,
    LeftToRight = 4,
    RightToLeft = 8,
    
    Diagonal = DiagonalUp | DiagonalDown | ReverseDiagonalUp | ReverseDiagonalDown,
    DiagonalUp = 16,
    DiagonalDown = 32,
    ReverseDiagonalUp = 64,
    ReverseDiagonalDown = 128,

    All = Vertical | Horizontal | Diagonal,
}