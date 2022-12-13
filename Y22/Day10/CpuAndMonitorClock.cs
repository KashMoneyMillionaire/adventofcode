using Y22.Extensions;

namespace Y22.Day10;

[DayOf(10)]
public class CpuAndMonitorClock : IDaySolver
{
    public static object SolvePart1(string input)
    {
        return input.ReadLines()
                    .AsQueue()
                    .Map(i => new Cpu(i))
                    .Stream(cpu => cpu.Tick(), cpu => cpu.Done)
                    .Where(cpu => cpu.Cycle % 40 == 20)
                    .Sum(cpu => cpu.SignalStrength);
    }

    public static object SolvePart2(string input)
    {
        return SolvePart1(input);
    }
}

public class Cpu
{
    public int SpriteCenterX { get; private set; }
    public int Cycle { get; private set; }
    public int SignalStrength => SpriteCenterX * Cycle;
    public int DrawPosition => (Cycle - 1) % 40;
    public bool Done => !_instructions.Any() && _currentInstruction == null;

    private readonly Queue<string> _instructions;
    private string? _currentInstruction;

    public Cpu(Queue<string> instructions)
    {
        _instructions = instructions;
        SpriteCenterX = 1;
        Cycle = 1;
    }

    public void Tick()
    {
        Draw();
        
        Cycle++;

        if (_currentInstruction is null)
        {
            _currentInstruction = _instructions.Dequeue();
            return;
        }

        if (_currentInstruction is "noop")
        {
            _currentInstruction = _instructions.Any() ? _instructions.Dequeue() : null;
        }
        else if (_currentInstruction.StartsWith("addx"))
        {
            int inc = _currentInstruction.Split(" ")[1].Parse<int>();
            SpriteCenterX += inc;
            _currentInstruction = null;
        }
    }

    public override string ToString()
    {
        return $"X: {SpriteCenterX} - Clock: {Cycle} - SS: {SignalStrength}";
    }

    private void Draw()
    {
        bool shouldDraw = DrawPosition.IsWithin(SpriteCenterX, -1, +1);
        Console.Write(shouldDraw ? "#" : ".");
        if (DrawPosition == 39) Console.WriteLine();
    }
}
