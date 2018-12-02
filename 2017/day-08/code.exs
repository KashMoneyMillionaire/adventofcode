defmodule Day8 do
    
    def run_instructions(list) do
        run_instructions(list, %{}, 0)
    end

    defp run_instructions([h|t], registers, max) do
        # binding() |> IO.inspect
        [register, instr, value, "if", checkKey, bool, condval] = String.split(h) 
          # |> IO.inspect

        valToAdjust = registers |> register_value(register)
        adjustSize = adjust_value(instr, value |> String.to_integer)

        checkValue = register_value(registers, checkKey)
          # |> IO.inspect

        if do_cmd?(checkValue, bool, condval |> String.to_integer) do
            newregs = Map.put(registers, register, valToAdjust + adjustSize)
            newmax = max(Map.values(newregs) |> Enum.max, max)
            run_instructions(t, newregs, newmax)
        else
            newmax = max(Map.values(registers) |> Enum.max(fn -> 0 end), max)
            run_instructions(t, registers, newmax)
        end
    end

    defp run_instructions([], registers, max) do
        { Map.values(registers) |> Enum.max, max }
    end

    defp adjust_value("inc", value) do
        value
    end

    defp adjust_value("dec", value) do
        -value
    end

    defp register_value(registers, key) do
        case registers[key] do
            nil -> 0
            val -> val
        end
    end

    defp do_cmd?(val1, bool, val2) do
        # binding() |> IO.inspect
    
        case bool do
            ">" -> val1 > val2                
            ">=" -> val1 >= val2                
            "<" -> val1 < val2                
            "<=" -> val1 <= val2
            "==" -> val1 == val2
            "!=" -> val1 != val2
        end
    end
end


"input.txt"
    |> File.read!
    |> String.split("\r\n")
    |> Day8.run_instructions
    |> IO.inspect