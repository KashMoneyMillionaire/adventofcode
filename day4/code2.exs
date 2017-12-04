"input.txt"
    |> File.read! # read in contents of file. Output: string -> string
    |> String.split("\n") # split all text by new line into rows. Output: string -> string[]
    |> Enum.map(fn(row) -> 
                    String.split(row) # split each row by whitespace. Output: string -> string[]
                        |> Enum.map(fn(i) -> 
                                        i 
                                            |> String.codepoints # turn string into char array. Output: string -> char[] (kinda)
                                            |> Enum.sort # sort the char array alphabetically. Output char[] -> char[]
                                            |> to_string # turn back into string. Output char[] -> string
                                    end) # sort each string in the row by its characters alphabetically
                end) 
    |> Enum.filter(fn(row) -> (Enum.uniq(row) |> length) == length(row) end) # keep items where the count of unique items is the same as the total number of items
    |> Enum.count # count number of rows left
    |> IO.inspect # output