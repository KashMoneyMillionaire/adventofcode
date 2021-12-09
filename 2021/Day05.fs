module Day05

open Utilities

type Point = { X: int; Y: int }
type Line = { From: Point; To: Point }

let solve () =

    let parseInput str =
        match str with
        | ParseRegex "(\d*),(\d*) -> (\d*),(\d*)" [ Integer x1; Integer y1; Integer x2; Integer y2 ] ->
            { From = { X = x1; Y = y1 }
              To = { X = x2; Y = y2 } }
        | _ -> failwith "bad input"

    let op st fn idx = if st < fn then st + idx else st - idx

    let points includeDiag =
        function
        | { From = { X = x1; Y = y1 }
            To = { X = x2; Y = y2 } } when x1 = x2 && y1 < y2 -> seq { for i in y1 .. y2 -> (x1, i) }
        | { From = { X = x1; Y = y1 }
            To = { X = x2; Y = y2 } } when x1 = x2 && y1 > y2 -> seq { for i in y2 .. y1 -> (x1, i) }
        | { From = { X = x1; Y = y1 }
            To = { X = x2; Y = y2 } } when y1 = y2 && x1 < x2 -> seq { for i in x1 .. x2 -> (i, y1) }
        | { From = { X = x1; Y = y1 }
            To = { X = x2; Y = y2 } } when y1 = y2 && x1 > x2 -> seq { for i in x2 .. x1 -> (i, y1) }
        | { From = { X = x1; Y = y1 }
            To = { X = x2; Y = y2 } } when includeDiag && abs ((y2 - y1) / (x2 - x1)) = 1 ->
            seq { for i in 0 .. abs (x1 - x2) -> ((op x1 x2 i), op y1 y2 i) }
        | _ -> Seq.empty

    let incr opt =
        opt |> Option.bind (fun x -> Some(x + 1))

    let addOrUpdate (map: Map<_, int>) point =
        if map.ContainsKey(point) then
            map.Change(point, incr)
        else
            map.Add(point, 1)

    let coverage points =
        (Map.empty<int * int, int>, points)
        ||> Seq.fold addOrUpdate
        |> Seq.filter (fun kv -> kv.Value >= 2)
        |> Seq.length

    let input =
        ReadInputLines "Day05" "input.txt"
        |> Seq.map parseInput

    let simplePoints =
        input |> Seq.map (points false) |> Seq.concat

    let diagPoints =
        input |> Seq.map (points true) |> Seq.concat

    printfn $"Part 1: {simplePoints |> coverage}"
    printfn $"Part 2: {diagPoints |> coverage}"
