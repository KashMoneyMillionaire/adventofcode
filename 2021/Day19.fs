module Day19

open System
open Utilities

let solve () =
    
    let parseCoordinates =
        function
        | ParseRegex "(-?\d+),(-?\d+),(-?\d+)" [Integer x; Integer y; Integer z;] -> (x,y,z)
        | _ -> failwith "bad input"
    
    let parseScanner lines =
        lines
        |> Seq.skip 1
        |> Seq.map parseCoordinates
        |> mapWith (fun (x,y,z) -> x,y,z)
    
    let inputs =
        ReadInput "Day19" "test.txt"
        |> split "\r\n\r\n"
        |> Seq.map (split "\r\n")
        |> Seq.map parseScanner
        |> Seq.map Seq.toList
        |> Seq.toList
    
    printfn $"Part 1: "
    printfn $"Part 2: "
