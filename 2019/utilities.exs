defmodule Utilities do
  def read_list_int() do
    "input.txt"
    |> File.read!()
    |> String.trim()
    |> String.split()
    |> Enum.take_while(fn x -> x != "#" end)
    |> Enum.map(&String.to_integer/1)
  end

  def read_list_int(split) do
    "input.txt"
    |> File.read!()
    |> String.trim()
    |> String.split(split)
    |> Enum.take_while(fn x -> x != "#" end)
    |> Enum.map(&String.to_integer/1)
  end
end
