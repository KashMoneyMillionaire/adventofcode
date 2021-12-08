module Day03

open Utilities

let solve () =

    let input =
        ReadInputLines "Day03" "input.txt"
        |> Seq.map Seq.toArray
        |> Seq.toArray

    let gammaBit (zero, one) = if zero > one then '0' else '1'
    let epsilonBit (zero, one) = if zero < one then '0' else '1'

    let gamma =
        input
        |> Seq.transpose
        |> Seq.map countBits
        |> Seq.map gammaBit
        |> Binary.parse

    let epsilon =
        input
        |> Seq.transpose
        |> Seq.map countBits
        |> Seq.map epsilonBit
        |> Binary.parse

    let rec filterRows2 preferred col (matrix: char [] []) =

        let possibleChars =
            getColumn col matrix
            |> Seq.countBy id
            |> Seq.sortByDescending snd
            |> (if preferred = '0' then Seq.rev else Seq.cache)
            |> Seq.take 2
            |> Seq.toArray
        
        let top = possibleChars[0]
        let next = possibleChars[1]
        
        let keepChar = if snd top = snd next then preferred else fst top
        
        let filtered =
            matrix
            |> Array.filter (fun row -> row.[col] = keepChar)

        if filtered.Length = 1 then filtered[0] else filterRows2 preferred (col + 1) filtered


    let oxy =
        input |> filterRows2 '1' 0 |> Binary.parse

    let co2 =
        input |> filterRows2 '0' 0 |> Binary.parse

    printfn $"Part 1: eps:%A{epsilon} gam:%A{gamma} pow:%A{epsilon * gamma}"
    printfn $"Part 2: oxy:%A{oxy} co2:%A{co2} life:%A{oxy * co2}"
