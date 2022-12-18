using Y22.Extensions;
using static System.Environment;

namespace Y22.Day11;

[DayOf(11)]
public class Day11 : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines(splitOn: $"{NewLine}{NewLine}")
                    .Select(Parse)
                    .ToList()
                    .DoTimes(DoRound, 20)
                    .OrderByDescending(m => m.Inspections)
                    .Take2()
                    .Map(m => m.Item1.Inspections * m.Item2.Inspections);
    }

    private static List<Monkey> DoRound(List<Monkey> monkeys)
    {
        foreach (var monkey in monkeys)
        {
            // Inspect
            // Relief
            // Worry
            // Throw
            while (monkey.HasItem(out var item))
            {
                item = monkey.Inspect(item);
                item /= 3; // relief
                if (monkey.Worry(item, out int throwTo))
                    monkeys[throwTo].ReceiveThrow(item);
            }
        }

        return monkeys;
    }

    private static Monkey Parse(string input)
    {
        string[] lines = input.Split(NewLine);
        var items = lines[1].Split(": ").Skip(2).Select(int.Parse);
        var op = lines[2].Split("old ")[1];
        var test = lines[3].Split("by ")[1];
        var t = lines[4].Split("monkey ")[1];
        var f = lines[5].Split("monkey ")[1];
        
        return new Monkey(items, op, test, t, f);
    }

    public static object SolvePart2(string input)
    {
        throw new NotImplementedException();
    }
}

internal class Monkey
{
    private readonly Operation _operation;
    private readonly Queue<int> _items;
    private readonly int _worryFactor;
    private readonly int _worryTrue;
    private readonly int _worryFalse;

    public int Inspections { get; private set; }

    public Monkey(IEnumerable<int> items, string op, string test, string @true, string @false)
    {
        _items = items.AsQueue();
        _operation = new(op);
        
        _worryFactor = test.Parse<int>();
        _worryTrue = @true.Parse<int>();
        _worryFalse = @false.Parse<int>();
    }

    public int Inspect(int result)
    {
        Inspections++;
        return _operation.Do(result);
    }

    public bool HasItem(out int result)
    {
        return _items.TryDequeue(out result);
    }

    public bool Worry(int result, out int monkeyIndex)
    {
        bool isWorried = result % _worryFactor == 0;
        monkeyIndex = isWorried ? _worryTrue : _worryFalse;
        return isWorried;
    }

    public void ReceiveThrow(int item)
    {
        _items.Enqueue(item);
    }
}

internal class Operation
{
    private readonly Func<int, int, int> _operator;
    private readonly string _operateWith;

    private static int Add(int i, int j) => i + j;
    private static int Multiple(int i, int j) => i * j;

    public Operation(string op)
    {
        string[] bits = op.Split(" ");
        _operator = bits[0] == "+" ? Add : Multiple;
        _operateWith = bits[1];
    }

    public int Do(int result)
    {
        return _operator(result, GetWith(result));
    }

    private int GetWith(int result)
    {
        return _operateWith == "old" ? result : _operateWith.Parse<int>();
    }
}