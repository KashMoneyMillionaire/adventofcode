module Day10

open Utilities

let solve () =

    let scores =
        seq {
            (')', ('(', 3, 1))
            (']', ('[', 57, 2))
            ('}', ('{', 1197, 3))
            ('>', ('<', 25137, 4))
        }
        |> dict

    let starters = Seq.map fst3 scores.Values
    let starter c = fst3 scores.[c]
    let badScore c = snd3 scores.[c]
    let goodScore c = third3 scores.[c]
    let closer c = scores |> Seq.find (fun kvp -> (fst3 kvp.Value) = c) |> (fun kvp -> kvp.Key)

    let scoreCombine scores =
        (0L, scores) ||> Seq.fold (fun acc next -> acc * 5L + (int64 next))
    
    let handleLine symbols =
        let handleChar (failed, prev: char seq) (curr: char) =
            if failed then
                (true, prev)
            else if Seq.contains curr starters then
                (false, prev |> append curr)
            else if (Seq.head prev) = (starter curr) then
                (false, prev |> Seq.skip 1)
            else
                (true, prev |> append curr)

        ((false, Seq.empty<char>), symbols) ||> Seq.fold handleChar

    let getClosing symbols =
        symbols |> Seq.map closer
    
    let lineResults =
        ReadInputLines "Day10" "input.txt"
        |> Seq.map Seq.toList
        |> Seq.map handleLine
    
    let badLinesScore =
        lineResults
        |> Seq.filter fst
        |> Seq.map snd
        |> Seq.map Seq.head
        |> Seq.map badScore
        |> Seq.sum

    let goodLinesScore =
        lineResults
        |> filterNot fst
        |> Seq.map snd
        |> Seq.map getClosing
        |> mapDeep goodScore
        |> Seq.map scoreCombine
        |> Seq.sort
        |> Seq.toList
        |> middleItem
    
    printfn $"Part 1: {badLinesScore}"
    printfn $"Part 2: {goodLinesScore}"
