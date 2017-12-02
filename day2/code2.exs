defmodule Day2 do

    def find_div_index(l) when is_list(l) do
        x = l |> Enum.find(fn(i) -> has_div?(l, i) end)
        y = l |> Enum.find(fn(i) -> i !== x and has_div?(l, i) end)
        {x, y}
    end

    defp has_div?(l, i) do
        l |> Enum.any?(fn(a) -> i !== a and (rem(i, a) == 0 or rem(a, i) == 0) end)
    end

    def div({a, b}) do
        if a > b do
            a / b
        else
            b / a             
        end
    end

end

IO.inspect "input.txt"
    |> File.read!
    |> String.split("\r\n")
    |> Enum.map(fn(row) -> String.split(row) 
        |> Enum.map(&String.to_integer/1)
    end)
    |> Enum.map(&Day2.find_div_index/1)
    |> Enum.map(&Day2.div/1)
    |> Enum.sum
