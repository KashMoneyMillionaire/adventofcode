﻿module Utilities

open System

let SplitLinesSplitOn (day: string) (splitBy: char) =
    System.IO.File.ReadLines("input/" + day + "/input.txt")
    |> Seq.map (fun x -> x.Split(splitBy))
    |> Seq.toList

let ReadInputLines day filename =
    System.IO.File.ReadLines("input/" + day + "/" + filename)

let stringAsInt: string -> int = int

let charsAsInt: char list -> int =
    fun (chars: char list) ->
        let str = new System.String(List.toArray chars)
        stringAsInt str

let numToChars x = x |> string |> Seq.toList

let logAndReturn map item =
    printfn (map item)
    item

let logAndContinue iter map = iter |> Seq.map (logAndReturn map)

let pairwise (offset: int) (source: seq<'T>) =
    let start = seq { yield! source }
    let next = seq { yield! source } |> Seq.skip offset

    Seq.zip start next

let split (splitOn: string) (str: string) =
    str.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)

type Binary =
    static member parse(str) = Convert.ToInt32(str, 2)

    static member parse(chars) =
        chars |> Seq.toArray |> String |> Binary.parse

let countBits (bits: seq<char>) =
    let countBit (zero, one) newChar =
        match newChar with
        | '0' -> (zero + 1, one)
        | '1' -> (zero, one + 1)
        | _ -> failwith "Unexpected bit value"

    bits |> Seq.fold countBit (0, 0)

let startsWith check items = Seq.head items = check

let getColumn c (matrix: _ [] []) = matrix |> Array.map (fun x -> x.[c])


let getRow c (matrix: _ [] []) = matrix |> Array.skip c |> Array.head

let matrixMap (mapping: int -> int -> 'a -> 'b) (matrix: 'a seq seq) =
    matrix
    |> Seq.mapi (fun i r -> r |> Seq.mapi (mapping i))

let inline fst4 (x, _, _, _) = x
let inline snd4 (_, x, _, _) = x
let inline third4 (_, _, x, _) = x
let inline fourth4 (_, _, _, x) = x

let mapWith (mapper: 'a -> 'b) (items: 'a seq) = items |> Seq.map (fun item -> (item, mapper item))