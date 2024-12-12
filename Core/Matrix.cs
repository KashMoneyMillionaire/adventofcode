
using System.Buffers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Core;

public static class MatrixExtensions
{
    public static char[][] ToMatrix(this Input input)
    {
        return input.Lines.Select(l => l.ToCharArray()).ToArray();
    }

    public static char[][] Rotate90Degrees(this char[][] matrix)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;
        char[][] rotated = new char[cols][];

        for (int i = 0; i < cols; i++)
        {
            rotated[i] = new char[rows];
            for (int j = 0; j < rows; j++)
            {
                rotated[i][j] = matrix[rows - j - 1][i];
            }
        }

        return rotated;
    }

    public static IEnumerable<SearchResult> Search(this char[][] matrix, string search, SearchDirections directions)
    {
        var rotated = directions.HasFlag(SearchDirections.Vertical) ? matrix.Rotate90Degrees() : [];
        var reverseSearch = new string(search.Reverse().ToArray());
        var searchLength = search.Length;

        if (directions.HasFlag(SearchDirections.LeftToRight.DebugMe()))
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row][col + i]);
                    }

                    if (word.ToString() == search)
                        yield return new(col, row, SearchDirections.LeftToRight);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.RightToLeft.DebugMe()))
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row][col + i]);
                    }

                    if (word.ToString() == reverseSearch)
                        yield return new(col + searchLength - 1, row, SearchDirections.RightToLeft);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.TopToBottom.DebugMe()))
        {
            for (int row = 0; row <= matrix.Length - searchLength; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row + i][col]);
                    }

                    if (word.ToString() == search)
                        yield return new(col, row, SearchDirections.TopToBottom);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.BottomToTop.DebugMe()))
        {
            for (int row = 0; row <= matrix.Length - searchLength; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row + i][col]);
                    }

                    if (word.ToString() == reverseSearch)
                        yield return new(col, row + searchLength - 1, SearchDirections.BottomToTop);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.DiagonalDown.DebugMe()))
        {
            for (int row = 0; row <= matrix.Length - searchLength; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row + i][col + i]);
                    }

                    if (word.ToString() == search)
                        yield return new(col, row, SearchDirections.DiagonalDown);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.DiagonalUp.DebugMe()))
        {
            for (int row = searchLength - 1; row < matrix.Length; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row - i][col + i]);
                    }

                    if (word.ToString() == search)
                        yield return new(col, row, SearchDirections.DiagonalUp);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.ReverseDiagonalUp.DebugMe()))
        {
            for (int row = 0; row <= matrix.Length - searchLength; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row + i][col + i]);
                    }

                    if (word.ToString() == reverseSearch)
                        yield return new(col + 3, row + 3, SearchDirections.ReverseDiagonalUp);
                }
            }
        }

        if (directions.HasFlag(SearchDirections.ReverseDiagonalDown.DebugMe()))
        {
            for (int row = searchLength - 1; row < matrix.Length; row++)
            {
                for (int col = 0; col <= matrix[row].Length - searchLength; col++)
                {
                    var word = new StringBuilder();
                    for (int i = 0; i < searchLength; i++)
                    {
                        word.Append(matrix[row - i][col + i]);
                    }

                    if (word.ToString() == reverseSearch)
                        yield return new(col + 3, row - 3, SearchDirections.ReverseDiagonalDown);
                }
            }
        }

    }

    public static IEnumerable<SearchResult> Search(this char[][] matrix, char[][] search)
    {
        var searches = search.Iterate(0, 4)
                             .Select((s) => s.Item.Do(s.Idx, m => m.Rotate90Degrees()))
                             .ToList();

        // For each of our search terms, we have to search
        for (int searchIdx = 0; searchIdx < searches.Count; searchIdx++)
        {
            var thisSearch = searches[searchIdx];
            thisSearch.AsString().Debug();

            // We search by iterating over the search space
            for (int row = 0; row <= matrix.Length - thisSearch.Length; row++)
                for (int col = 0; col <= matrix[row].Length - thisSearch[0].Length; col++)
                {
                    // Now we scan the current search matrix
                    var match = true;
                    for (int sRow = 0; sRow < thisSearch.Length && match; sRow++)
                        for (int sCol = 0; sCol < thisSearch[sRow].Length && match; sCol++)
                        {
                            var x = row + sRow;
                            var y = col + sCol;
                            if (thisSearch[sRow][sCol] == '.')
                            {
                                $"Skipping [{x},{y}]={matrix[x][y]}".Debug();
                                continue;
                            }

                            if (matrix[x][y] != thisSearch[sRow][sCol])
                            {
                                $"Bad character matrix[{x},{y}]={matrix[x][y]}, thisSearch[{sRow},{sCol}]={thisSearch[sRow][sCol]}".Debug();
                                match = false;
                            }
                        }

                    if (match)
                        yield return new(col, row, SearchDirections.All);
                }
        }

    }

    public static string AsString(this char[][] matrix)
    {
        return matrix.Select(m => m.Join("")).Join("\n");
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