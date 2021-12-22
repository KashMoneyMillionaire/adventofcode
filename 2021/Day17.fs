module Day17

open Utilities

let solve () =

    let order ((x1, x2), (y1, y2)) =
        (min x1 x2, max x1 x2), (min y1 y2, max y1 y2)
    
    let parse =
        function
        | ParseRegex "target area: x=(-?\d+)..(-?\d+), y=(-?\d+)..(-?\d+)" [ Integer x1; Integer x2; Integer y1; Integer y2 ] ->
            (x1, x2), (y1, y2)
        | _ -> failwith "bad input"

    let maxFromCross ((minX, maxX), (minY, maxY)) trajectory =
        let trajectory =
            seq {
                let mutable currX, currY = 0,0
                let mutable velX, velY = trajectory
                while currX < maxX && minY < currY do
                    currX <- currX + velX
                    currY <- currY + velY
                    velX <- velX + if velX > 0 then -1 else if velX < 0 then 1 else 0
                    velY <- velY - 1
                    
                    yield (currX, currY)
            }
            |> Seq.toList
        
        if trajectory |> Seq.exists (fun (x,y) -> minX <= x && x <= maxX && minY <= y && y <= maxY) then
            Some (trajectory |> Seq.map snd |> Seq.max)
        else
            None
    
    let target =
        ReadInputLines "Day17" "input.txt"
        |> Seq.head
        |> parse
        |> order
        
    let possibleTrajectories =
        seq { -700 .. 700 } >< seq { -700 .. 700 }
        |> Seq.map (maxFromCross target)
        |> optionFilter
        
    let maxAltitude = Seq.max possibleTrajectories
    let totalOptions = Seq.length possibleTrajectories
    
    printfn $"Part 1: {maxAltitude}"
    printfn $"Part 2: {totalOptions}"
