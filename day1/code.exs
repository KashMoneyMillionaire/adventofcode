
defmodule Day1 do

    def sum([], a) do
        a
    end

    def sum([_], a) do
        a
    end

    def sum([h,h | t], a) do
        sum([h | t], a + (String.to_integer h))
    end

    def sum([_ | t], a) do
        sum(t, a)
    end
    
end

input = "input.txt" |> File.read! |> String.split("")

[h | _] = input
["" | reverse] = Enum.reverse input

sum = Day1.sum([h | reverse], 0)

IO.puts sum
