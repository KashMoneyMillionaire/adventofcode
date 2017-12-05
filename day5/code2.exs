"input.txt"
    |> File.read! # read in contents of file. Output: string -> string
    |> String.split("\n") # split all text by new line. Output: string[]
    