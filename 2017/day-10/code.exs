defmodule Day10 do
    def loop(instr, {loop, skip, offset}) do



        double = Enum.concat(loop, loop)
        sub = Enum.slice(double, offset..(offset+instr-1)
            |> Enum.reverse

        

        part1 = Enum.slice(loop, offset..(offset+instr-1))
        part2 = Enum.slice(0)

            #2,1,0,3,4 ,2,1,0,3,4
            #2,1,0,(3,4 ,2,1),0,3,4

        
    end

    defp split(list, start, size) do
        
    end
end

# loop = Enum.to_list(0.255)
# instrs = String.split("63,144,180,149,1,255,167,84,125,65,188,0,2,254,229,24", ",", trim: true)
#     |> Enum.map(&String.to_integer/1)

loop = [0,1,2,3,4]
instrs = [63,144,180,149,1,255,167,84,125,65,188,0,2,254,229,24]

Enum.reduce(instrs, {loop, 0, 0}, &Day10.loop/2)