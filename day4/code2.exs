"input.txt"
    |> File.read!
    |> String.split("\n")
    |> Enum.map(fn(r) -> String.split(r) |> Enum.map(fn(i) -> i |> String.codepoints |> Enum.sort |> to_string end) end)
    |> Enum.filter(fn(r) -> (Enum.uniq(r) |> length) == length(r) end)
    |> Enum.count
    |> IO.inspect