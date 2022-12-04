using Y22.Extensions;

namespace Y22.Day02;

[DayOf(02)]
public class RockPaperScissors : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .Select(ParseRound1)
                    .Select(ScoreRound)
                    .Sum();
    }

    public static object SolvePart2(string input)
    {
        return input.ReadLines()
                    .Select(ParseRound2)
                    .Select(ScoreRound)
                    .Sum();
    }

    private static int ScoreRound((Shoot Opponent, Shoot Self) round)
    {
        return (int)round.Self + ScoreShoot(round.Opponent, round.Self);
    }

    private static int ScoreShoot(Shoot opponent, Shoot self)
    {
        if (GetWinner(opponent) == self)
            return 6;

        if (GetLoser(opponent) == self)
            return 0;
        
        return 3;
    }

    private static (Shoot, Shoot) ParseRound1(string roundInput)
    {
        if (roundInput is not [var opponent, ' ', var self])
            throw new($"Bad input line: {roundInput}");

        return (GetFromInput(opponent), GetFromInput(self));
    }

    private static (Shoot, Shoot) ParseRound2(string roundInput)
    {
        if (roundInput is not [var opponent, ' ', var outcome])
            throw new($"Bad input line: {roundInput}");

        var opponentShoot = GetFromInput(opponent);
        var neededShot = outcome switch
        {
            'X' => GetLoser(opponentShoot),
            'Y' => opponentShoot,
            'Z' => GetWinner(opponentShoot),
            _ => throw new("Bad input letter")
        };
        
        return (opponentShoot, neededShot);
    }

    static Shoot GetFromInput(char input)
    {
        return input switch
        {
            'A' or 'X' => Shoot.Rock,
            'B' or 'Y' => Shoot.Paper,
            'C' or 'Z' => Shoot.Scissors,
            _ => throw new("Bad input letter")
        };
    }
    
    private static Shoot GetWinner(Shoot opponent) => opponent switch
    {
        Shoot.Paper => Shoot.Scissors,
        Shoot.Rock => Shoot.Paper,
        Shoot.Scissors => Shoot.Rock,
        _ => throw new("Bad input shoot")
    };
    
    private static Shoot GetLoser(Shoot opponent) => opponent switch
    {
        Shoot.Paper => Shoot.Rock,
        Shoot.Rock => Shoot.Scissors,
        Shoot.Scissors => Shoot.Paper,
        _ => throw new("Bad input shoot")
    };
}

internal enum Shoot
{
    Rock = 1, Paper, Scissors 
}