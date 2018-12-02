defmodule Day10 do
    require Bitwise

    def npos(pos, h, ss, len) when pos + h + ss < len do
            pos + h + ss
    end
    def npos(pos, h, ss, len), do: npos(pos - len, h, ss, len)

    def step(instrs, loop, pos \\ 0, ss \\ 0)
    def step([h|rst], loop, pos, ss) do
        len = Enum.count(loop)
        if (h + pos) < len do
            nlst = Enum.reverse_slice(loop, pos, h)
            step(rst, nlst, npos(pos, h, ss, len), ss + 1)
        else
            borrow = h + pos - len
            {fst, snd} = Enum.split(loop, borrow)
            tmplst = snd ++ fst
            tmprev = Enum.reverse_slice(tmplst, pos - borrow, h)
            {fst, snd} = Enum.split(tmprev, len - borrow)
            nlst = snd ++ fst
            step(rst, nlst, npos(pos, h, ss, len), ss + 1)
        end
    end
    def step([], loop, pos, ss), do: {loop, pos, ss}

    def simple_hash(instrs, loop) do
        {loop,_,_} = step(instrs, loop)
        Enum.take(loop, 2)
        |> Enum.reduce(1, &(&1 * &2))
    end

    def to_ascii(str), do: String.to_charlist(str)

    def add_secret(loop) do
        Enum.concat(loop, [17,31,73,47,23])
    end

    def step64(instrs, loop \\ 0..255, round \\ 64, pos \\ 0, step \\ 0)
    def step64(instrs, loop, round, pos, ss) when round > 0 do
        {nlst, pos, ss} = step(instrs, loop, pos, ss)
        step64(instrs, nlst, round - 1, pos, ss)
    end
    def step64(_vls, loop, _round, _pos, _ss), do: loop

    def xor_down(loop), do: Enum.reduce(loop, 0, &Bitwise.bxor/2)

    def to_dense_hash(loop) do
        Enum.chunk_every(loop, 16)
        |> Enum.map(&xor_down/1)
    end

    def normalize(str) do
        down = String.downcase(str)
        if String.length(str) == 2 do
            down
        else
            "0" <> down
        end
    end

    def to_hexstring(loop) do
        Enum.map(loop, &(Integer.to_string(&1, 16)))
        |> Enum.map(&normalize/1)
        |> Enum.join
    end

    def hash(str) do
        str
            |> String.to_charlist
            |> add_secret
            |> step64
            |> to_dense_hash
            |> to_hexstring
    end
end

# File.read!("input")
#     |> String.trim
#     |> String.split(",")
#     |> Enum.map(&String.to_integer/1)
[63,144,180,149,1,255,167,84,125,65,188,0,2,254,229,24]
    |> Day10.simple_hash(0..255)
    |> IO.inspect

"63,144,180,149,1,255,167,84,125,65,188,0,2,254,229,24"
    |> String.trim
    |> Day10.hash
    |> IO.inspect