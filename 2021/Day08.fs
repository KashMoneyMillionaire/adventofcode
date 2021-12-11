module Day08

open System
open Utilities

let solve () =

    let uniqueLengths =
        seq {
            2
            3
            4
            7
        }

    let intersects search places full =
        let sub = search |> Seq.except full
        Seq.length sub = (Seq.length search - places)

    let contains search full =
        intersects search (Seq.length search) full

    let notin searchIn searching = not (Seq.contains searching searchIn)

    let buildMapper (symbols: string seq) =
        let sortedSymbols = symbols |> Seq.map sortStr |> Seq.cache
        
        let one = sortedSymbols |> Seq.find (ofLength 2)

        let four = sortedSymbols |> Seq.find (ofLength 4)

        let seven = sortedSymbols |> Seq.find (ofLength 3)

        let eight = sortedSymbols |> Seq.find (ofLength 7)

        let three =
            sortedSymbols
            |> Seq.filter (ofLength 5)
            |> Seq.find (contains seven)

        let five =
            sortedSymbols
            |> Seq.filter (ofLength 5)
            |> filterNot (contains three)
            |> Seq.find (intersects four 3)

        let two =
            sortedSymbols
            |> Seq.filter (ofLength 5)
            |> Seq.find (notin (seq { three; five }))
           
        let nine =
            sortedSymbols
            |> Seq.filter (ofLength 6)
            |> Seq.find (contains four)

        let zero =
            sortedSymbols
            |> Seq.filter (ofLength 6)
            |> filterNot (contains four)
            |> Seq.find (contains seven)

        let six =
            sortedSymbols
            |> Seq.filter (ofLength 6)
            |> Seq.find (notin (seq { nine; zero }))

        let getNum from =
            match sortStr from with
            | str when str = one -> '1'
            | str when str = two -> '2'
            | str when str = three -> '3'
            | str when str = four -> '4'
            | str when str = five -> '5'
            | str when str = six -> '6'
            | str when str = seven -> '7'
            | str when str = eight -> '8'
            | str when str = nine -> '9'
            | str when str = zero -> '0'
            | _ -> failwith "uh oh"

        getNum

    let getNumber (outs: string []) (mapper: string -> char) =
        outs
        |> Seq.map mapper
        |> String.Concat
        |> int

    let signals, outputs =
        ReadInputLines "Day08" "input.txt"
        |> Seq.takeWhile (fun line -> line <> "#")
        |> Seq.map (split " | ")
        |> Seq.map paired
        |> Seq.map (fun p -> (split " " (fst p), split " " (snd p)))
        |> unzip

    let uniques =
        outputs
        |> mapDeep (fun item -> item.Length)
        |> mapMany id
        |> Seq.filter (fun len -> Seq.contains len uniqueLengths)
        |> Seq.length

    let displaySum =
        (signals, outputs)
        ||> Seq.zip
        |> Seq.map (fun (s, o) -> buildMapper s |> getNumber o)
        |> Seq.sum

    printfn $"Part 1: {uniques}"
    printfn $"Part 2: {displaySum}"
