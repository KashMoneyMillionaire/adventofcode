defmodule Twenty18_Day1_Part2 do
  defp recurse([h | t], sum, freqs) do
    cond do
      Enum.member?(freqs, h + sum) -> h + sum
      true -> recurse(t ++ [h], sum + h, freqs ++ [sum + h])
    end
  end

  def full do
    "lib/day-01/input.txt"
    |> File.read!()
    |> String.split()
    |> Enum.map(&String.to_integer/1)
    |> recurse(0, [0])
  end

  def sample do
    [-6, +3, +8, +5, -6]
    |> recurse(0, [0])
  end
end

defmodule Aoc.Year2018.Day01.ChronalCalibration do
  def part_1(input) do
    input
    |> String.split("\n")
    |> Enum.reduce(0, fn x, acc ->
      {i, ""} = Integer.parse(x)
      i + acc
    end)
  end

  def part_2(input, start_freq \\ 0, prev_freqs \\ %{0 => true}) do
    res =
      input
      |> String.split()
      |> Enum.reduce_while({start_freq, prev_freqs}, fn x, {freq, prev_freqs} ->
        {i, ""} = Integer.parse(x)
        freq = i + freq

        if prev_freqs[freq] do
          {:halt, {:succ, freq}}
        else
          {:cont, {freq, Map.put(prev_freqs, freq, true)}}
        end
      end)

    case res do
      {:succ, freq} -> freq
      {freq, prev_freqs} -> part_2(input, freq, prev_freqs)
    end
  end
end
