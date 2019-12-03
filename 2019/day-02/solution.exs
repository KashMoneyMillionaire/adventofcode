Code.require_file("utilities.exs", "..")

defmodule Intcode do
  def run(map) do
    run(map, 0)
  end

  defp run(map, position) do
    op = map[position]
    run(map, position, op)
  end

  defp run(map, position, 1) do
    operate(map, position, &Kernel.+/2)
  end

  defp run(map, position, 2) do
    operate(map, position, &Kernel.*/2)
  end

  defp run(map, _, 99) do
    map[0]
  end

  defp operate(map, position, operation) do
    pos1 = map[position + 1]
    pos2 = map[position + 2]
    place = map[position + 3]
    val1 = map[pos1]
    val2 = map[pos2]
    new_value = operation.(val1, val2)
    new_map = Map.put(map, place, new_value)
    # IO.inspect(map)
    # IO.inspect([val1, val2, place])
    # IO.inspect(new_map)
    run(new_map, position + 4)
  end
end

input = Utilities.read_list_int(",")

mem =
  0..(length(input) - 1)
  |> Stream.zip(input)
  |> Enum.into(%{})

# Part 1
mem
|> Map.merge(%{1 => 12, 2 => 2})
|> Intcode.run()
|> IO.puts()

# Part 2
opt =
  for i <- 0..99,
      j <- 0..99,
      do: %{1 => i, 2 => j}

for m <- opt do
  val =
    Map.merge(mem, m)
    |> Intcode.run()

  if val == 19_690_720, do: m, else: nil
end
|> Enum.filter(&(!is_nil(&1)))
|> Enum.at(0)
|> case do
  %{1 => noun, 2 => verb} -> 100 * noun + verb
end
|> IO.inspect()
