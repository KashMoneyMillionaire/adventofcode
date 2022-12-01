namespace Year2021;

public static class Day24
{
    public static void Solve()
    {
        var steps = File.ReadAllLines("input/Day24/input.txt").ToArray();
        var commandChunks = steps.Chunk(14).ToArray();

        var cache = new Dictionary<(int, int, int, int, int), (int, int, int, int)>();

        var possibles = PossibleModelNumbers();

        for (long i = 11111111111111; i <= 99999999999999; i++)
        {
            if (i % 10 == 0)
                continue;

            if (i / 10 % 10 == 0)
                continue;

            if (i % 1000 == 111)
                Console.WriteLine(i);
            
            int[] possibleModelNumbers = i.ToString().Select(ch => ch - '0').ToArray();
            var possibleModel = new Stack<int>(possibleModelNumbers);
            
            
            var (w, x, y, z) = (0, 0, 0, 0);
            for (int hunkIndex = 0; hunkIndex < 14; hunkIndex++)
            {
                string[] commands = commandChunks[hunkIndex];

                for (int commandIndex = 0; commandIndex < 14; commandIndex++)
                {
                    var key = (w, x, y, z, j: commandIndex);
                    if (!cache.ContainsKey(key))
                        cache[key] = Calculate((w, x, y, z), commands, new(possibleModel));

                    (w, x, y, z) = cache[key];
                }
            }
            
        

            // i += (long)Math.Pow(10, 13 - step);            
            if (z == 0)
            {
                var model = possibleModel.Select(i => (char)(i + '0')).ToArray();
                Console.WriteLine(new string(model));
            }
        }
        
        
        Console.WriteLine();

        /*var ps = new (int, int, int)[14];
        for (int i = 0; i < 14; i++)
        {
            ps[i] = (int.Parse(new string(steps[4 + i * 18]).Skip(6).ToArray()),
                     int.Parse(new(steps[5 + i * 18].Skip(6).ToArray())),
                     int.Parse(new(steps[15 + i * 18].Skip(6).ToArray())));
        }

        (long? lowest, long? highest) = (long.MaxValue, long.MinValue);

        for (long i = 10000000000000; i <= 99999999999999; i++)
        {
            var digits = i.ToString().Select(ch => int.Parse(ch.ToString())).ToArray();
            long z = 0;

            for (int step = 0; step < ps.Length; step++)
            {
                (int p1, int p2, int p3) = ps[step];
                var w = digits[step];
                var test = (z % 26) + p2 == w;
                if (w != 0 && p1 == 26 && test)
                {
                    z /= p1;
                }
                else if (w != 0 && p1 == 1 && !test)
                {
                    z = 26 * (z / p1) + w + p3;
                }
                else
                {
                    //e.g. 234560000 to 234569999
                    i += (long)Math.Pow(10, 13 - step);
                    i--;
                    break;
                }
            }

            if (z == 0)
            {
                (lowest, highest) = (i < lowest ? i : lowest, i > highest ? i : highest);
            }
        }
        
        Console.Write((lowest, highest));*/
    }

    private static (int w, int x, int y, int z) Calculate(
        (int w, int x, int y, int z) state,
        string[] commands,
        Stack<int> possibleModel)
    {
        foreach (string command in commands)
        {
            var (w, x, y, z) = state;
            if (command.StartsWith("inp"))
            {
                state = (possibleModel.Pop(), x, y, z);
                continue;
            }

            if (command.StartsWith("mul"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) * v);
                continue;
            }

            if (command.StartsWith("div"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) / v);
                continue;
            }

            if (command.StartsWith("mod"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) % v);
                continue;
            }

            if (command.StartsWith("add"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) + v);
                continue;
            }

            if (command.StartsWith("eql"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) == v ? 1 : 0);
            }
            
            if (command.StartsWith("neq"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v) != v ? 1 : 0);
            }
            
            if (command.StartsWith("set"))
            {
                char r = command[4];
                char v = command[6];

                state = Set(state, r, Get(state, v));
            }
        }

        return state;
    }

    private static (int, int, int, int) Set((int w, int x, int y, int z) state, char r, int v)
    {
        return r switch
        {
            'w' => (v, state.x, state.y, state.z),
            'x' => (state.w, v, state.y, state.z),
            'y' => (state.w, state.x, v, state.z),
            'z' => (state.w, state.x, state.y, v),
        };
    }

    private static int Get((int w, int x, int y, int z) state, char val)
    {
        return val switch
        {
            'w' => state.w,
            'x' => state.x,
            'y' => state.y,
            'z' => state.z,
            _ => val - '0'
        };
    }

    private static IEnumerable<Stack<int>> PossibleModelNumbers()
    {
        for (long i = 99999999999999; i >= 10000000000000; i--)
        {
            if (i % 10 == 0)
                continue;

            if (i / 10 % 10 == 0)
                continue;

            int[] possibleModelNumbers = i.ToString().Select(ch => ch - '0').ToArray();
            var stack = new Stack<int>(possibleModelNumbers);
            yield return stack;
        }
    }
}