module Day13

open Utilities

let solve () =

    let parseCommands str =
        match str with
        | ParseRegex "(\d+),(\d+)" [ Integer x; Integer y ] -> (x, y)
        | _ -> failwith "bad input"
    
    let parseFolds str =
        match str with
        | ParseRegex "fold along ([xy])=(\d+)" [ Char direction; Integer axis ] -> (direction, axis)
        | _ -> failwith "bad input"
    
    let foldHorizontal (dots: (int*int) seq) line =
        let twice = line * 2
        dots
        |> Seq.map (fun (x, y) -> if y > line then (x, twice - y) else (x,y))
    
    let foldVertical (dots: (int*int) seq) line =
        let twice = line * 2
        dots
        |> Seq.map (fun (x, y) -> if x > line then (twice - x, y) else (x,y))
    
    let foldDots dots command =
        match command with
        | ('x', dir) -> foldVertical dots dir
        | ('y', dir) -> foldHorizontal dots dir
        | _ -> failwith "bad command"
    
    let dots, folds =
        ReadInputLines "Day13" "input.txt"
        |> splitSeq ""
        |> pairMap (parseCommands, parseFolds)
    
    let firstFold = foldDots dots (Seq.head folds)
    
    let finalDots =
        (dots, folds)
        ||> Seq.fold foldDots
        |> Seq.distinct
        |> Seq.toList

    let maxX = folds |> Seq.filter (fun (dir, _) -> dir = 'x') |> Seq.minBy snd |> snd
    let maxY = folds |> Seq.filter (fun (dir, _) -> dir = 'y') |> Seq.minBy snd |> snd
        
    printfn $"Part 1: {Seq.length firstFold}"
    printfn $"Part 2: {maxX} by {maxY}"

    seq {0 .. maxY - 1}
    |> Seq.iter (fun y ->
        printf "\n"
        seq {0 .. maxX - 1}
        |> Seq.iter (fun x ->
            if Seq.contains (x,y) finalDots then
                printf "#"
            else
                printf "."
            )
        )
    printf "\n"