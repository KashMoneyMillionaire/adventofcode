module Day01

open Utilities.Ut

let solve () =
    let input =
        ReadInputLines "Day01" "input.txt" |> Seq.map int

    let solution1 =
        input
        |> pairwise 1
        |> Seq.filter (fun (x, y) -> x < y)
        |> Seq.length

    let solution2 =
        input
        |> pairwise 3
        |> Seq.filter (fun (x, y) -> x < y)
        |> Seq.length

    printfn $"Part 1: %A{solution1}"
    printfn $"Part 2: %A{solution2}"
