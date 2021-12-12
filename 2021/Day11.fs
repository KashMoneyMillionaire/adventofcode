module Day11

open Utilities

let solve () =

    let willFlash (point: MatrixPoint<int>) =
        point.Value >= 10
    
    let inc (point: MatrixPoint<int>) =
        point.Value <- point.Value + 1
        
    let incAdjacent (c: MatrixPoint<int>) =
        seq {
            c.Bottom
            c.Top
            c.Left
            c.Right
            c.Right |> Option.bind (fun x -> x.Top)
            c.Right |> Option.bind (fun x -> x.Bottom)
            c.Left |> Option.bind (fun x -> x.Top)
            c.Left |> Option.bind (fun x -> x.Bottom)
        }
        |> optionFilter
        |> Seq.iter inc

    let notSynced (matrix: Matrix<int>) =
        matrix |> Seq.concat |> Seq.exists (fun i -> i.Value <> 0)
    
    let resetFlashed (matrix: Matrix<int>) =
        matrix
        |> Seq.concat
        |> Seq.iter (fun c -> if c.Value > 9 then c.Update(0))
    
    let rec propagateFlash (previouslyFlashed: MatrixPoint<int> list) (matrix: Matrix<int>) =
        let needToFlash =
            matrix
            |> Seq.concat
            |> Seq.filter willFlash
            |> Seq.filter (fun a -> not (Seq.contains a previouslyFlashed))
            |> Seq.toList
        
        if any needToFlash then
            needToFlash |> Seq.iter incAdjacent
                        
            let currCount = Seq.length needToFlash
            
            let allFlashed = (List.append previouslyFlashed needToFlash)
            let subCount = propagateFlash allFlashed matrix
            
            currCount + subCount
        else
            0

    let cycle (prevCount, matrix: Matrix<int>) idx =
        
        // Increment
        matrix
            |> Seq.concat
            |> Seq.iter inc
        
        // Flash
        let flashCount = propagateFlash List.empty matrix

        // Reset
        resetFlashed matrix

        printf $"After step {idx}:\n"
        print matrix |> ignore
        
        (prevCount + flashCount, matrix)

    printf "Before any steps:\n"
    
    let points =
        ReadInputLines "Day11" "input.txt"
        |> buildMatrix
        |> print

    let flashes, _ =
        ((0, points), seq { 1 .. 100 })
        ||> Seq.fold cycle

    let mutable count = 101
    while notSynced points do
        let _ = cycle (0, points) count
        count <- count + 1
    
    printfn $"Part 1: {flashes}"
    printfn $"Part 2: {count}"
