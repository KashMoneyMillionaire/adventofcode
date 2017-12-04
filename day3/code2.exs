defmodule Day2 do

    def value_n({x, y}) do
        if x > 0 and y >= 0 do
            value_n()
        end
    end
    
    def value_n({0, 0}) do
        1
    end
    
end