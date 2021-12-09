﻿module Utilities

open System
open System.Collections.Generic

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

let split splitOn (str: string) = str.Split(splitOn)

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

let getColumn c (matrix: _[][]) =
    matrix
    |> Array.map (fun x -> x[c])
    

let getRow c (matrix: _[][]) =
    matrix
    |> Array.skip c
    |> Array.head
   
let seqDict (src:seq<'a * 'b>) = 
    let d = new Dictionary<'a, 'b>()
    for k,v in src do
        d.Add(k,v)
    d

// get a seq of key-value pairs for easy iteration with for (k,v) in d do...
let pairs (d:Dictionary<'a, 'b>) =
    seq {
        for kv in d do
            yield (kv.Key, kv.Value)
    }
