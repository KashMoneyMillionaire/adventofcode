defmodule Day11 do

    def move_tile(direction, {{currX, currY, currZ}, _, furthestDist}) do
        case direction do
            "n" -> {currX, currY + 1, currZ - 1} |> distance_from_origin(furthestDist)
            "ne" -> {currX + 1, currY, currZ - 1} |> distance_from_origin(furthestDist)
            "se" -> {currX + 1, currY - 1, currZ} |> distance_from_origin(furthestDist)
            "s" -> {currX, currY - 1, currZ + 1} |> distance_from_origin(furthestDist)
            "sw" -> {currX - 1, currY, currZ + 1} |> distance_from_origin(furthestDist)
            "nw" -> {currX - 1, currY + 1, currZ} |> distance_from_origin(furthestDist)
        end
    end

    def distance_from_origin({currX, currY, currZ}, furthestDist) do
        currDistance = (abs(currX) + abs(currY) + abs(currZ)) / 2
        {{currX, currY, currZ}, currDistance, max(currDistance, furthestDist)}
    end

end

"input.txt"
    |> File.read!
    |> String.split(",", trim: true)
    |> Enum.reduce({{0,0,0},0,0}, &Day11.move_tile/2)
    |> IO.inspect