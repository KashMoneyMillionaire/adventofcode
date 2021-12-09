module Day06

open Utilities

let solve () =

    let daysToTrack = 18
    let firstSpawnRate = 9
    let secondSpawnRate = 7
    
    let rec fishAtTime =
        function
        | afterDays when afterDays < firstSpawnRate -> 1
        | afterDays when afterDays >= firstSpawnRate -> 1 + fishAtTime (afterDays - secondSpawnRate)
        | _ -> failwith "not possible?"
    
    let getSpawnLookup spawnTime =
        seq { 0 .. daysToTrack + (firstSpawnRate - spawnTime) }
        |> Seq.map (fun day -> (day, fishAtTime day))
        |> seqDict
    
    let getOverallSpawnTime spawnTime =
        getSpawnLookup spawnTime
        |> pairs
        |> Seq.last
        |> snd
    
    let fishSpawnRates =
        ReadInputLines "Day06" "test.txt"
        |> Seq.head
        |> split ","
        |> Seq.map int
        |> Seq.countBy id
        |> Seq.map (fun (spawnTime, fishCount) -> (*) (getOverallSpawnTime spawnTime) fishCount)
        |> Seq.sum

//    let buildMap spawnTime =
//        seq { (daysToTrack + spawnTime - firstSpawnRate) .. -1 .. 1 }
//        |> Seq.map fishGrowthIfBornWith
//    
//    let totalMap =
//        daySequence 
//        |> Seq.map (fun day -> (day, fishGrowthIfBornWith day))
//        |> dict
//
//    let fishSum =
//        fishSpawnRates
//        |> Seq.map (fun (rate, _) -> buildMap rate)
//        |> 
//        |> Seq.map (fun currFishRate -> totalMap.[currFishRate + firstSpawnRate])
//        |> Seq.sum
    
    printfn $"Part 1: {fishSpawnRates}"
    printfn $"Part 2: "
