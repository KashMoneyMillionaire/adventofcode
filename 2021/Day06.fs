module Day06

open System
open Utilities

let solve () =

    let matureSpawnRate = 7
    let youngSpawnRate = 9
    let mutable cache = Map.empty<int*int, int64>
    
    let rec numFishWilSingleWillSpawn daysLeft currentState =
        
        if cache.ContainsKey(daysLeft, currentState) then
            cache.[(daysLeft, currentState)]
        else
            let selfSpawnCount =
                (float (daysLeft - currentState)) / (float matureSpawnRate)
                |> Math.Ceiling
                |> int64
            
            let childSpawnCount =
                seq { (daysLeft - currentState - 1) .. -matureSpawnRate .. 0 }
                |> Seq.map (fun days -> numFishWilSingleWillSpawn days (youngSpawnRate - 1))
                |> Seq.sum
            
            let spawnTotal = (max selfSpawnCount 0L) + childSpawnCount
            cache <- cache.Add((daysLeft, currentState), spawnTotal)
            spawnTotal
        
    let originalSpawns =
        ReadInputLines "Day06" "input.txt"
        |> Seq.head
        |> split ","
        |> Seq.map int
        
    let totalFish80Days =
        originalSpawns
        |> Seq.map (numFishWilSingleWillSpawn 80)
        |> Seq.map ((+) 1L)
        |> Seq.sum
    
    let totalFish256Days =
        originalSpawns
        |> Seq.map (numFishWilSingleWillSpawn 256)
        |> Seq.map ((+) 1L)
        |> Seq.sum
    
    printfn $"Part 1: {totalFish80Days}"
    printfn $"Part 2: {totalFish256Days}"
