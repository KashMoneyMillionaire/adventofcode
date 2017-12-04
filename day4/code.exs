"input.txt"
    |> File.read!
    |> String.split("\n")
    |> Enum.map(fn(r) -> String.split(r) end)
    |> Enum.filter(fn(r) -> (Enum.uniq(r) |> length) == length(r) end)
    |> Enum.count
    |> IO.inspect