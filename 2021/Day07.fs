module Day07

open Utilities

let solve () =

    let positions =
        ReadInputLines "Day07" "input.txt"
        |> Seq.head
        |> split ","
        |> Array.map float

    let fuelCost (n: float) = n * (n + 1.0) / 2.0

    let floorCost =
        positions
        |> Seq.map (fun x -> x - floor (Seq.average positions))
        |> Seq.map abs
        |> Seq.map fuelCost
        |> Seq.sum

    let ceilCost =
        positions
        |> Seq.map (fun x -> x - ceil (Seq.average positions))
        |> Seq.map abs
        |> Seq.map fuelCost
        |> Seq.sum

    let crabMedian =
        positions
        |> Seq.map (fun p -> abs (median positions - p))
        |> Seq.sum

    printfn $"Part 1: sum: {crabMedian}"
    printfn $"Part 2: {floorCost} {ceilCost}"
