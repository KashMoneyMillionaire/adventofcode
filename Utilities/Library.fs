namespace Utilities

open System.Collections
open Microsoft.FSharp.Primitives.Basics

module Ut =
    let SplitLinesSplitOn (day: string) (splitBy: char) =
        System.IO.File.ReadLines(day + "/input.txt")
        |> Seq.map (fun x -> x.Split(splitBy))
        |> Seq.toList

    let ReadInputLines day filename =
        System.IO.File.ReadLines(day + "/" + filename)

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