"lib/day-01/input.txt"
|> File.read!()
|> String.split()
|> Enum.map(&String.to_integer/1)
|> Enum.sum
|> IO.puts
