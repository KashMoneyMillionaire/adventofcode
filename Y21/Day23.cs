namespace Year2021;

public class Day23
{
    public static void Solve()
    {
        var lines = File.ReadAllLines(@"input/day23/input.txt");

        var (spaces, amphipods) = Parser.ParseInput(lines);

        var mover = new Mover(spaces);
        var minDistance = mover.Move(amphipods);

        Console.WriteLine($"Part 1: {minDistance}");
        Console.WriteLine($"Part 2: ");
    }
}

internal class Mover
{
    private readonly Space[,] _spaces;
    private long _minTotalEnergy = long.MaxValue;
    private List<Amphipod> _finishedAmphipods;

    public Mover(Space[,] spaces)
    {
        _spaces = spaces;
    }

    public long? Move(List<Amphipod> amphipods)
    {
        var minEnergy = long.MaxValue;
        foreach (var amphipod in amphipods)
        {
            var placesToMove = _spaces.Cast<Space>()
                                      .Where(s => s.CanMove(amphipod, amphipods))
                                       // .OrderByDescending(s => s.Coordinates.X == 4)
                                      .ToList();
            if (!placesToMove.Any())
                continue;

            long minMove = long.MaxValue;
            foreach (var placeToMove in placesToMove)
            {
                var moveCost = Move(amphipod, placeToMove, amphipods);
                if (moveCost is null)
                    continue;

                minMove = Math.Min(minMove, moveCost.Value);
            }

            if (minMove == long.MaxValue)
                continue;

            minEnergy = Math.Min(minEnergy, minMove);
        }

        if (minEnergy == long.MaxValue)
            return null;

        _minTotalEnergy = Math.Min(_minTotalEnergy, minEnergy);
        return minEnergy;
    }

    private long? Move(Amphipod amphipod, Space newSpace, List<Amphipod> amphipods)
    {
        var a2 = amphipod with
        {
            Path = amphipod.Path.Append(newSpace).ToList(),
            StepsTaken = amphipod.StepsTaken + newSpace.Distance(amphipod.Location),
            Location = newSpace.Coordinates
        };

        var newAmphipods = amphipods.Select(a => a == amphipod ? a2 : a).ToList();

        long currentEnergyExpended = newAmphipods.Sum(a => a.EnergyExpended);
        if (currentEnergyExpended > _minTotalEnergy)
            return null;

        if (newAmphipods.All(a => a.IsHome))
        {
            _finishedAmphipods = newAmphipods;
            return currentEnergyExpended;
        }

        // _spaces.Print(amphipods, newAmphipods);
        return Move(newAmphipods);
    }
}

internal static class Parser
{
    public static (Space[,] spaces, List<Amphipod> amphipods) ParseInput(string[] lines)
    {
        var chars = lines.TakeWhile(l => l != "#")
                         .Select(l => l.ToCharArray())
                         .ToArray();

        var spaces = new Space[13, 5];
        var starts = new List<(int X, int Y)>();

        for (int y = 0; y < chars.Length; y++)
        {
            char[] rows = chars[y];
            for (int x = 0; x < rows.Length; x++)
            {
                spaces[x, y] = new(rows[x], (x, y), spaces);

                if (rows[x].OneOf('A', 'B', 'C', 'D'))
                    starts.Add((x, y));
            }
        }

        var amphipods = starts.Select(p => new Amphipod(chars[p.Y][p.X], p, new() { spaces[p.X, p.Y] }, 0))
                               // .OrderBy(a => a.MovementBase)
                               // .ThenByDescending(a => a.Location.X)
                              .ToList();

        return (spaces, amphipods);
    }
}

public record Space
{
    private readonly Space[,] _spaces;
    public (int X, int Y) Coordinates { get; }

    public bool IsFloor { get; }
    public bool IsSpace { get; }
    public bool IsWall => !IsFloor && !IsSpace;
    public bool IsRoom { get; }
    public bool IsHallway => Coordinates.IsHallway();
    public bool IsEntrance { get; }
    public char? RoomFor { get; }
    public bool IsTopRoom => IsRoom && Coordinates.Y == 2;
    public bool IsBottomRoom => IsRoom && Coordinates.Y == 3;

    public Space(char c, (int X, int Y) coordinates, Space[,] spaces)
    {
        _spaces = spaces;

        IsFloor = c is not '#' and not ' ';
        IsSpace = c is ' ';
        IsRoom = coordinates.X.OneOf(3, 5, 7, 9) && coordinates.Y.OneOf(2, 3);
        RoomFor = !IsRoom
            ? null
            : coordinates.X switch
            {
                3 => 'A',
                5 => 'B',
                7 => 'C',
                9 => 'D',
                _ => throw new("bad char")
            };
        Coordinates = coordinates;
        IsEntrance = new[] { (3, 1), (5, 1), (7, 1), (9, 1) }.Contains(coordinates);
    }

    public Space GetAdjacentSpace()
    {
        return _spaces[Coordinates.X, Coordinates.Y.Opposite(2, 3)];
    }

    public Amphipod? FilledWith(List<Amphipod> amphipods)
    {
        return amphipods.FirstOrDefault(a => a.Location == Coordinates);
    }

    private bool IsFilled(List<Amphipod> amphipods)
    {
        return FilledWith(amphipods) is not null;
    }

    public bool CanMove(Amphipod a, List<Amphipod> amphipods)
    {
        if (a.Location == Coordinates)
            return false;

        if (IsWall)
            return false;

        if (IsSpace)
            return false;

        if (IsEntrance)
            return false;

        // Can't move from hallway to hallway
        if (a.Location.IsHallway() && IsHallway)
            return false;

        if (IsRoom)
        {
            if (RoomFor != a.Type)
                return false;

            var adjacentSpace = GetAdjacentSpace();
            var adjacentAmphipod = adjacentSpace.FilledWith(amphipods);

            // Can't move into a room where the other thing in the room is wrong type
            if (adjacentAmphipod is not null && adjacentAmphipod.Type != a.Type)
                return false;

            // Can't move into the top space of a room unless below is occupied
            if (IsTopRoom && adjacentAmphipod is null)
                return false;
        }

        var movingFrom = _spaces[a.Location.X, a.Location.Y];

        // Can't move from the top home room if bottom is filled with same type
        if (movingFrom.IsHomeRoomFor(a)
            && movingFrom.IsTopRoom
            && BottomRoom(a.Type).IsFilledWithType(a.Type, amphipods))
            return false;

        if (movingFrom.IsHomeRoomFor(a) && movingFrom.IsBottomRoom)
            return false;

        // Can't move into an occupied space
        if (FilledWith(amphipods) is not null)
            return false;

        if (AnyInTheWay(movingFrom, this, amphipods))
            return false;

        return true;
    }

    private bool AnyInTheWay(Space movingFrom, Space movingTo, List<Amphipod> amphipods)
    {
        var movingAmphipod = amphipods.Single(a => a.Location == movingFrom.Coordinates);
        var startX = movingFrom.Coordinates.X;
        var endX = movingTo.Coordinates.X;

        if (amphipods.Any(a => a.Location.IsHallway() && a.Location.X.IsBetween(startX, endX)))
            return true;

        if (movingFrom.IsBottomRoom && amphipods.Any(a => a.Location.Y == movingAmphipod.Location.Y - 1))
            return true;

        return false;
    }

    private bool IsFilledWithType(char aType, List<Amphipod> amphipods)
    {
        return FilledWith(amphipods)?.Type == aType;
    }

    private Space BottomRoom(char amphipodType)
    {
        return amphipodType switch
        {
            'A' => _spaces[3, 3],
            'B' => _spaces[5, 3],
            'C' => _spaces[7, 3],
            'D' => _spaces[9, 3],
            _ => throw new ArgumentOutOfRangeException(nameof(amphipodType), amphipodType, null)
        };
    }

    private static Space TopRoom(char amphipodType, Space[,] spaces)
    {
        return amphipodType switch
        {
            'A' => spaces[3, 2],
            'B' => spaces[5, 2],
            'C' => spaces[7, 2],
            'D' => spaces[9, 2],
            _ => throw new ArgumentOutOfRangeException(nameof(amphipodType), amphipodType, null)
        };
    }

    private bool IsHomeRoomFor(Amphipod amphipod)
    {
        return IsRoom && RoomFor == amphipod.Type;
    }

    private bool IsBelow(Space space)
    {
        return space.Coordinates.Y < Coordinates.Y;
    }

    public long Distance((int X, int Y) coordinates)
    {
        var (fromX, fromY) = coordinates;
        var (toX, toY) = Coordinates;

        int horizontalDist = Math.Abs(fromX - toX);
        int verticalDist = Math.Abs(1 - fromY) + Math.Abs(1 - toY);
        return horizontalDist + verticalDist;
    }
}

public record Amphipod(char Type, (int X, int Y) Location, List<Space> Path, long StepsTaken)
{
    public long EnergyExpended => MovementBase * StepsTaken;


    public long MovementBase => Type switch
    {
        'A' => 1,
        'B' => 10,
        'C' => 100,
        'D' => 1000,
        _ => throw new("bad char")
    };

    public bool IsHome => Type switch
    {
        'A' => Location.X == 3 && Location.Y.OneOf(2, 3),
        'B' => Location.X == 5 && Location.Y.OneOf(2, 3),
        'C' => Location.X == 7 && Location.Y.OneOf(2, 3),
        'D' => Location.X == 9 && Location.Y.OneOf(2, 3),
        _ => throw new("bad char")
    };

    public bool IsFinished => Type switch
    {
        'A' => Location.X == 3 && Location.Y == 3,
        'B' => Location.X == 5 && Location.Y == 3,
        'C' => Location.X == 7 && Location.Y == 3,
        'D' => Location.X == 9 && Location.Y == 3,
        _ => throw new("bad char")
    };
}