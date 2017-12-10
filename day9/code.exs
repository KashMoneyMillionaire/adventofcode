defmodule Day9 do
  
    def process_stream([], _, _, accum) do
        accum
    end

    def process_stream(["{" | stream], :score, score, accum) do
        process_stream(stream, :score, score + 1, accum)
    end

    def process_stream(["}" | stream], :score, score, {scoreCount, garbageCount}) do
        process_stream(stream, :score, score - 1, {scoreCount + score, garbageCount})
    end

    def process_stream(["," | stream], :score, score, accum) do
        process_stream(stream, :score, score, accum)
    end

    def process_stream(["<" | stream], :score, score, accum) do
        process_stream(stream, :garbage, score, accum)
    end

    def process_stream([">" | stream], :garbage, score, accum) do
        process_stream(stream, :score, score, accum)
    end

    def process_stream(["!" | [_ | stream]], :garbage, score, accum) do
        process_stream(stream, :garbage, score, accum)
    end

    def process_stream([_ | stream], :garbage, score, {scoreCount, garbageCount}) do
        process_stream(stream, :garbage, score, {scoreCount, garbageCount + 1})
    end

end

"input.txt"
    |> File.read!
    |> String.split("", trim: true)
    |> Day9.process_stream(:score, 0, {0, 0})
    |> IO.inspect