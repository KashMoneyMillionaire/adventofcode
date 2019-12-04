module Day04

open Utilities.Ut

let solve =
    let input = "357253-892942" |> Seq.toList
    let [bot, '-', top] = input

    printfn "%A %A" bot top