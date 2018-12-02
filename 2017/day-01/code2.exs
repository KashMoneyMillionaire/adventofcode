
input = "input.txt" |> File.read! |> String.split("")

["" | reverse] = Enum.reverse input

half = Enum.count(reverse) / 2 |> round
chunks = Enum.chunk reverse, half

tup = Enum.zip chunks
# IO.puts tup
same = Enum.filter(tup, fn({a, b}) -> a == b end)
l = Enum.map(same, fn({a, a}) -> String.to_integer a end)
sum = Enum.sum l

IO.puts sum * 2
