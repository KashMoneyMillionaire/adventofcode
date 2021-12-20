module Day15

open FSharpx.Collections
open Utilities

let solve () =

    let adjacentPoints (x,y) (maxX, maxY) =
        seq {
            (x+1,y)
            (x-1,y)
            (x,y+1)
            (x,y-1)
        }
        |> Seq.filter(fun (l,r) -> 0 <= l && l <= maxX && 0 <= r && r <= maxY)
    
    // Dijkstra's-ish
    let traverse (items: int seq seq) =
        
        let matrix =
            items
            |> Seq.map Seq.toArray
            |> Seq.toArray
        let height, width = (matrix.Length, matrix.[0].Length)
        let start = (0,0)
        let finish = (width - 1, height - 1)
        
        let mutable available = PriorityQueue.empty false |> PriorityQueue.insert (0, start)
        let mutable visited = Set.empty
        let mutable distances = Map.empty
        
        while not available.IsEmpty do

            let (dist, curr), newAvail = PriorityQueue.pop available
            available <- newAvail
            
            if not (Set.contains curr visited) then
                visited <- Set.add curr visited
                
                adjacentPoints curr finish
                |> Seq.iter (fun adj ->
                    if not (Set.contains adj visited) then
                        let x, y = adj
                        let total = dist + matrix.[x].[y]
                        
                        if not (Map.containsKey adj distances) || total < distances.[adj] then
                            distances <- distances |> Map.change adj (fun _ -> Some total)
                            available <- PriorityQueue.insert (total, adj) available                    
                    )

        let travelDist = distances.[finish]
        travelDist
                
    let lines = ReadInputLines "Day15" "input.txt"
    
    let partialMap =
        lines
        |> Seq.map stringSeq
        |> mapDeep int
        |> mapDeep (fun i -> i - int '0')
    
    let partialDist = traverse partialMap
    printfn $"Part 1: {partialDist}"
    
    let fullMap =
        partialMap
        |> expandMatrix 5 (fun (i: int) x ->
            let transform = i + (1 * x) - 1
            (transform % 9) + 1
            )

    let fullDist = traverse fullMap
    printfn $"Part 2: {fullDist}"
