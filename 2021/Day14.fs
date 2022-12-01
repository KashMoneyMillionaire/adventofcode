module Day14

open Microsoft.FSharp.Collections
open Utilities

let solve () =

    let parseInsertions line =
        match line with
        | ParseRegex "([A-Z])([A-Z]) -> (.*)" [Char l; Char r; Char insert] -> ((l, r), insert)
        | _ -> failwith "bad insertion rule"

    let lines = ReadInputLines "Day14" "input.txt"

    let template = lines |> Seq.head
    let translations =
        lines
        |> Seq.skip 2
        |> Seq.map parseInsertions
        |> Map

    let baseCount =
        template
        |> pairwise 1
        |> toMap (fun _ -> 1L)
    
    let incMap (map: Map<'a, int64>) (position: 'a) (value: int64) =
        map |> Map.change position (fun o ->
            match o with
            | Some v -> Some (v + value)
            | None -> Some value
            )
        
    let splitCount (counts:  Map<char * char, int64>) =

        let mutable newMap = Map.empty<char*char, int64>
        
        counts
        |> Map.iter (fun pair count ->
            let a,b = pair
            let c = translations.[pair]
            
            newMap <- incMap newMap (a,c) count
            newMap <- incMap newMap (c,b) count
            )
        
        newMap
        
    let finalCounts =
        (baseCount, seq { 1 .. 1 })
        ||> Seq.fold (fun counts _ -> splitCount counts)
        
    let charCounts =
        finalCounts
        |> Seq.map (fun l -> (fst l.Key, l.Value))
        |> Seq.fold (fun a b -> incMap a (fst b) (snd b)) Map.empty  

    let counts =
        charCounts
        |> Map.toSeq
        |> Seq.sortByDescending snd 
    
    let most = Seq.head counts
    let least = Seq.last counts
    
    printfn $"Part 1: {most} {least} {snd most - snd least}"
    printfn $"If most or least is {template |> Seq.last}, add 1"
