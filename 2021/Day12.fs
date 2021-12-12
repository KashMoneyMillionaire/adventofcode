module Day12

open Utilities

type Path = {Start: string; End: string;}

type Cave(name: string) =
    
    let mutable connectedCaves = List.empty<Cave>
    
    override this.ToString() = this.Name

    member this.Name = name
    member this.IsBigCave =
        match name with
        | ParseRegex "([A-Z])" [ String _ ] -> true
        | ParseRegex "([a-z])" [ String _ ] -> false
        | _ -> failwith "unexpected"
    
    member this.Caves = connectedCaves
    
    member this.Add(cave: Cave) =
        connectedCaves <- append cave connectedCaves |> Seq.toList

let solve () =

    let buildPath (caves: Cave list) =
        let beg =
            caves
            |> Seq.map (fun c -> $"{c.Name}")
        let fullList = seq { yield! beg; "end\n" }
        String.concat "," fullList
    
    let parsePaths str =
        match str with
        | ParseRegex "(.+)-(.+)" [ String start; String ending ] -> { Start = start; End = ending }
        | _ -> failwith "bad input"

    let buildCaves (paths: Path seq) =
        (Seq.empty<Cave>, paths)
        ||> Seq.fold (fun caves path ->
            let startCave = caves |> Seq.tryFind (fun c -> c.Name = path.Start)
            let endCave = caves |> Seq.tryFind (fun c -> c.Name = path.End)
            
            match (startCave, endCave) with
            | Some st, Some en ->
                st.Add(en)
                en.Add(st)
                caves
            | None, Some en ->
                let st = Cave(path.Start)
                st.Add(en)
                en.Add(st)
                seq { yield! caves; st }
            | Some st, None ->
                let en = Cave(path.End)
                st.Add(en)
                en.Add(st)
                seq { yield! caves; en }
            | None, None ->
                let st = Cave(path.Start)
                let en = Cave(path.End)
                st.Add(en)
                en.Add(st)
                seq { yield! caves; st; en }
            )
    
    let simpleCanVisit (cave: Cave) (visited: Cave list) =
        cave.IsBigCave || not(Seq.contains cave visited)
    
    let complexCanVisit (cave: Cave) (visited: Cave list) =
        let smallAlreadyVisitedTwice =
            visited
            |> Seq.filter (fun c -> not c.IsBigCave)
            |> Seq.countBy (fun c -> c.Name)
            |> Seq.filter (fun c -> snd c > 1)
            |> any
        let canVisit = cave.IsBigCave || not(Seq.contains cave visited) || not smallAlreadyVisitedTwice
        canVisit && cave.Name <> "start"
    
    let rec findPaths (curr: Cave) (ending: Cave) (visited: Cave list) canVisit =
        
        let newVisited = (seq { yield! visited; curr } |> Seq.toList)
        let availableCaves = curr.Caves |> Seq.filter (fun c -> canVisit c newVisited)
        
        if curr = ending then
            seq { visited }
        else if Seq.length availableCaves = 0 then
            Seq.empty
        else
            availableCaves
            |> mapMany (fun c -> findPaths c ending newVisited canVisit)

    let goesThroughSmall (caves: Cave seq) =
        caves
        |> Seq.exists (fun c -> not c.IsBigCave)
    
    let caves =
        ReadInputLines "Day12" "input.txt"
        |> Seq.map parsePaths
        |> buildCaves
    
    let st = Seq.find (fun (c: Cave) -> c.Name = "start") caves
    let en = Seq.find (fun (c: Cave) -> c.Name = "end") caves
    
    let smallTraversal = findPaths st en List.empty<Cave> simpleCanVisit
    let smallPaths = smallTraversal |> Seq.map goesThroughSmall |> Seq.length
    printfn $"Part 1: {smallPaths}"
        
    let bigTraversal = findPaths st en List.empty<Cave> complexCanVisit |> Seq.toList
//    bigTraversal |> Seq.map buildPath |> Seq.sort |> printAll
    printfn $"Part 2: {bigTraversal.Length}"
