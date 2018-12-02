IO.inspect "input.txt"
    |> File.read!
    |> String.split("\r\n")
    |> Enum.map(fn(row) -> String.split(row) 
        |> Enum.map(&String.to_integer/1)
    end)
    |> Enum.map(fn(rowArry) -> Enum.max(rowArry) - Enum.min(rowArry) end)
    |> Enum.sum