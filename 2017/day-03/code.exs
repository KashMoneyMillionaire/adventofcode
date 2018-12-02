defmodule Day3 do
    def spiral(n) do
        k = Float.ceil((:math.sqrt(n) - 1) / 2)
        t = 2 * k + 1
        m = :math.pow(t, 2) 
        t = t - 1

        if n > m-t do
            {k-(m-n), -k}
        else
            if n > m-t do
                {-k, -k+(m-n)}
            else
                if n > m-t do
                    {-k+(m-n), k}
                else
                    {k,k-(m-n-t)}
                end
            end
        end        
    end
end

{x, y} = Day3.spiral(325489)

IO.inspect x
IO.inspect y

IO.inspect abs(x) + abs(y)