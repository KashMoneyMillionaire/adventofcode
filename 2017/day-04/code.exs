"input.txt"
    |> File.read! # read in contents of file. Output: string -> string
    |> String.split("\n") # split all text by new line. Output: string[]
    |> Enum.map(fn(row) -> String.split(row) end) # split each row by whitespace. Output: string[][]
    |> Enum.filter(fn(row) -> (Enum.uniq(row) |> length) == (row |> length) end) # keep items where the count of unique items is the same as the total number of items
    |> Enum.count # count number of rows left
    |> IO.inspect # output