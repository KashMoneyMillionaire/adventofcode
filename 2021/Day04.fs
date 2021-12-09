module Day04

open Utilities

type Board(flatRows: (int * int * int * bool) seq, move: int option) =
    let marked = flatRows |> Seq.filter fourth4

    let anyRowSolutions =
        marked
        |> Seq.countBy fst4
        |> Seq.map snd
        |> Seq.exists ((=) 5)

    let anyColSolutions =
        marked
        |> Seq.countBy snd4
        |> Seq.map snd
        |> Seq.exists ((=) 5)
    let isSolved = anyRowSolutions || anyColSolutions

    member this.Debug =
        flatRows
        |> Seq.map (fun (_, _, i, isMarked) -> if isMarked then $"_{i,2}_" else $" {i,2} ") 
        |> Seq.chunkBySize 5
        |> Seq.map (String.concat " ")
        |> String.concat "\n"

    member this.Rows = flatRows
    
    member this.WinningCall = move |> Option.bind(fun m -> if isSolved then Some m else None)

    member this.UnmarkedSum =
        flatRows
        |> Seq.filter (fourth4 >> not)
        |> Seq.sumBy third4
    
    new(rowsInit: int seq seq) =
        let buildRows =
            rowsInit |> matrixMap (fun x y i -> (x, y, i, false)) |> Seq.concat

        Board(buildRows, None)

    new(board: Board, move: int) =

        let getRow =
            function
            | x, y, cell, _ when cell = move -> (x, y, cell, true)
            | tup -> tup

        let newRows = board.Rows |> Seq.map getRow
        Board(newRows, Some move)

let solve () =

    let buildBoard (lines: seq<string>) =
        let splitAndConvert line = split " " line |> Seq.map int

        lines
        |> Seq.take 5
        |> Seq.map splitAndConvert
        |> Board

    let parseInput lines =
        let moves =
            lines |> Seq.head |> split "," |> Array.map int

        let boards =
            lines
            |> Seq.skip 2
            |> Seq.chunkBySize 6
            |> Seq.map buildBoard

        (boards, moves)

    let buildNew move (board: Board) = Board(board, move)
    let isWinner (board: Board) = board.WinningCall.IsSome

    let untilFirstWinner (inputsForBoards: seq<seq<Board>>) =
        inputsForBoards
            |> mapWith (Seq.exists isWinner)
            |> Seq.find snd
            |> fst
            |> Seq.find isWinner

    let untilLastWinner (inputsForBoards: seq<seq<Board>>) =
        let diff (pairs: seq<Board> * seq<Board>) =
            Seq.zip (fst pairs) (snd pairs)
            |> Seq.filter (fun (l, _) -> l.WinningCall.IsNone)
            |> Seq.head
            |> snd
        
        inputsForBoards
            |> mapWith (Seq.forall isWinner)
            |> Seq.pairwise
            |> Seq.filter (fun (l, r) -> snd l <> snd r)
            |> Seq.map (fun (l, r) -> (fst l, fst r))
            |> Seq.head
            |> diff

    let boardsOverTime =
        ReadInputLines "Day04" "input.txt"
        |> parseInput
        ||> Seq.scan (fun existing newMove -> existing |> Seq.map (buildNew newMove))
    
    let winningBoard = boardsOverTime |> untilFirstWinner
    let losingBoard = boardsOverTime |> untilLastWinner

    let winningScore =
        match winningBoard.WinningCall with
        | Some i -> i * winningBoard.UnmarkedSum
        | _ -> failwith "no call found"
    
    let losingScore =
        match losingBoard.WinningCall with
        | Some i -> i * losingBoard.UnmarkedSum
        | _ -> failwith "no call found"
    
    printfn $"Part 1: winning call: %A{winningBoard.WinningCall} unmarked: %A{winningBoard.UnmarkedSum} score: %A{winningScore}"
    printfn $"Part 2: losing call: %A{losingBoard.WinningCall} unmarked: %A{losingBoard.UnmarkedSum} score: %A{losingScore}"
