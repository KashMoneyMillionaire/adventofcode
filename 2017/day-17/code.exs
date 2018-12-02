defmodule Day17 do

    def cycle(0, []) do
        [0]
    end

    def cycle(x, acc) do
        move = 3 + x
        adjusted_move = rem(move, length(acc))
        "move: #{move}, adjusted_move: #{adjusted_move}" |> IO.inspect
        List.insert_at(acc, adjusted_move, x)
    end

    def find_next([9, x | _]) do
        x
    end

    def find_next([_ | t]) do
        find_next(t)
    end
end

0..9
    |> Enum.reduce([], &Day17.cycle/2)
    |> IO.inspect(limit: :infinity)
    |> Day17.find_next
    |> IO.inspect