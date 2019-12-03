defmodule Twenty18_Day1_Part2 do
  def x([h | t], sum, freqs) do
    cond do
      Enum.member?(freqs, h + sum) -> h + sum
      true -> x(t ++ [h], sum + h, freqs ++ [sum + h])
    end
  end

  def full do
    "lib/day-01/input.txt"
    |> File.read!()
    |> String.split()
    |> Enum.map(&String.to_integer/1)
    |> Twenty18_Day1_Part2.x(0, [])
    |> IO.puts()
  end

  def sample do
    [-6, +3, +8, +5, -6]
    |> Twenty18_Day1_Part2.x(0, [])
    |> IO.puts()
  end
end
