namespace Utilities

open System
open System.Collections
open Microsoft.FSharp.Primitives.Basics

module Ut =
    let SplitLinesSplitOn (day: string) (splitBy: char) =
        System.IO.File.ReadLines("input/" + day + "/input.txt")
        |> Seq.map (fun x -> x.Split(splitBy))
        |> Seq.toList

    let ReadInputLines day filename =
        System.IO.File.ReadLines("input/" + day + "/" + filename)

    let stringAsInt: string -> int = int

    let charsAsInt: char list -> int =
        fun (chars: char list) ->
            let str = new System.String(List.toArray (chars))
            stringAsInt (str)

    let numToChars x = x |> string |> Seq.toList

    let logAndReturn map item =
        printfn (map item)
        item

    let logAndContinue iter map = iter |> Seq.map (logAndReturn map)

    let pairwise (offset: int) (source: seq<'T>) =
        let start = seq { yield! source }
        let next = seq { yield! source } |> Seq.skip offset

        Seq.zip start next

    let split splitOn (str: string) = str.Split(splitOn)

    type Binary =
        static member parse(str) = Convert.ToInt32(str, 2)
        static member parse(chars) = chars |> Seq.toArray |> String |> Binary.parse

    let countBits (bits: seq<char>) =
        let countBit (zero, one) newChar =
            match newChar with
            | '0' -> (zero + 1, one)
            | '1' -> (zero, one + 1)
            | _ -> failwith "Unexpected bit value"

        bits |> Seq.fold countBit (0, 0)

    let mostPopular preferred items =
        items
        |> Seq.countBy id
        |> Seq.sortByDescending (fun x -> snd (x), fst (x) = preferred)
        |> Seq.head
        |> fst

    let startsWith check items = Seq.head (items) = check
