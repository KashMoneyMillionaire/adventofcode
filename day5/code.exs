defmodule Day5 do

    def jump([h | t]) do
        jump([], h, t, h, 0)
    end

    def jump(front, currentItem, back, jumpSize, iterationCount) do

        IO.inspect front
        IO.inspect currentItem
        IO.inspect back
        IO.inspect jumpSize
        IO.inspect iterationCount
        IO.puts "--\n"

        if length(back) < jumpSize do
            iterationCount
        else

            # (0) 3 0 1 -3      0
            # (1) 3 0 1 -3      1
            # 2 (3) 0 1 -3      2
            # 1 4 0 1 (-3)      3
            # 1 (4) 0 1 -2      4
            # exit              5

            cond do
                jumpSize == 0 ->
                    jump(front, currentItem + 1, back, currentItem, iterationCount + 1)
                jumpSize < 0 -> 
                    {f, m, b} = three_parts(front, jumpSize)
                    jump(f, m, b ++ back, 0, iterationCount)
                jumpSize > 0 ->
                    {f, m, b} = three_parts(back, jumpSize)
                    jump(front ++ f, m, b, 0, iterationCount)
            end
        end
    end

    defp three_parts([h | t], 0) do
        IO.puts "three_parts/2 0"
        { [], h, t }
    end

    defp three_parts([h | t], n) do
        IO.puts "three_parts/2 n"
        three_parts([], h, t, n)
    end

    defp three_parts(front, middle, [bh | bt], 1) do
        IO.puts "three_parts/4 1"
        IO.inspect front, label: "front"
        IO.inspect middle, label: "middle"
        IO.inspect bh, label: "bh"
        IO.inspect bt, label: "bt"
        IO.puts "--\n"
        { front ++ [middle], bh, bt }
    end

    defp three_parts(front, middle, [bh | bt], n) do
        IO.puts "\nthree_parts/4"
        IO.inspect front, label: "front"
        IO.inspect middle, label: "middle"
        IO.inspect bh, label: "bh"
        IO.inspect bt, label: "bt"
        IO.inspect n, label: "n"
        IO.puts "--\n"
        three_parts(front ++ [middle], bh, bt, n-1 )
    end

end

directions = "input.txt"
    |> File.read! # read in contents of file. Output: string -> string
    |> String.split("\r\n") # split all text by new line. Output: string[]
    |> Enum.map(&String.to_integer/1)

# IO.inspect directions

Day5.jump(directions)
    |> IO.inspect