module Day25

open Utilities

let solve () =

    let buildArray (lines: char[][]) =
        Array2D.init lines[0].Length lines.Length (fun x y -> lines[y][x])
    
    let arraySeq (array: char[,]) =
        seq { 0 .. array.GetLength(0) - 1 } >< seq { 0 .. array.GetLength(1) - 1 }
        |> Seq.toList
    
    let isRight (array: char[,]) (x,y) =
        array[x,y] = '>'
    
    let isDown (array: char[,]) (x,y) =
        array[x,y] = 'v'
    
    let canGoRight (array: char[,]) (x,y) =
        array[(x + 1) % array.GetLength(0), y] = '.'
    
    let canGoDown (array: char[,]) (x,y) =
        array[x, (y + 1) % array.GetLength(1)] = '.'
    
    let findMovingRight (array: char[,]) =
        arraySeq array
        |> Seq.filter (isRight array)
        |> Seq.toList
        |> Seq.filter (canGoRight array)
    
    let findMovingDown (array: char[,]) =
        arraySeq array
        |> Seq.filter (isDown array)
        |> Seq.toList
        |> Seq.filter (canGoDown array)
    
    let moveRight (array: char[,]) movingRight =
        movingRight
        |> Seq.toList
        |> Seq.iter (fun (x,y) ->
            array[x,y] <- '.'
            array[(x + 1) % array.GetLength(0), y] <- '>'
            )
    
    let moveDown (array: char[,]) movingDown =
        movingDown
        |> Seq.toList
        |> Seq.iter (fun (x,y) ->
            array[x,y] <- '.'
            array[x, (y + 1) % array.GetLength(1)] <- 'v'
            )
    
    let move (array: char[,]) =
        let movingRight = findMovingRight array |> Seq.toList
        moveRight array movingRight
        let movingDown = findMovingDown array |> Seq.toList
        moveDown array movingDown
        
        Seq.length movingRight + Seq.length movingDown
    
    let array = 
        ReadInputLines "Day25" "input.txt"
        |> Seq.map Seq.toArray
        |> Seq.toArray
        |> buildArray

    let mutable doMove = true
    let mutable i = 0
    while doMove do
//        if seq { 0;1;2;3;4;5;10;20;30;40;50;55;56;57;58; } |> Seq.contains i then
//            printf $"After {i} steps:\n"
//            printM2 array
            
        let r = move array
        doMove <- r > 0
        i <- i + 1
    
//    printf $"After {i} steps:\n"
//    printM2 array
    
    printfn $"Part 1: {i}"
    printfn $"Part 2: "
