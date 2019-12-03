defmodule Day1Tests do
  use ExUnit.Case
  doctest Twenty18_Day1_Part2

#   @tag :skip
#   test "run part 2 sample" do
#     out = Twenty18_Day1_Part2.sample()
#     assert out == 5
#   end

#   @tag :skip
#   test "run part 2 full" do
#     out = Twenty18_Day1_Part2.full()
#     assert out == 5
#   end

  test "other code" do
    input =
      "lib/day-01/input.txt"
      |> File.read!()

    assert Aoc.Year2018.Day01.ChronalCalibration.part_2(input) == {:succ, 1}
  end
end
