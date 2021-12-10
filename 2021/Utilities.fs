module Utilities

open System
open System.Collections.Generic
open System.Text.RegularExpressions

let SplitLinesSplitOn (day: string) (splitBy: char) =
    System.IO.File.ReadLines("input/" + day + "/input.txt")
    |> Seq.map (fun x -> x.Split(splitBy))
    |> Seq.toList

let ReadInputLines day filename =
    System.IO.File.ReadLines("input/" + day + "/" + filename)
    |> Seq.takeWhile (fun line -> line <> "#")

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

let split (splitOn: string) (toSplit: string) =
    toSplit.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)

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

let (|ParseRegex|_|) regex str =
    let m = Regex(regex).Match(str)

    if m.Success then
        Some(List.tail [ for x in m.Groups -> x.Value ])
    else
        None
        
let (|Integer|_|) (str: string) =
   let mutable intVal = 0
   if Int32.TryParse(str, &intVal) then Some(intVal)
   else None

let inline median items = items |> Array.sort |> (fun arr -> arr.[items.Length / 2])

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

let paired twoItemSeq =
    (twoItemSeq |> Seq.head, twoItemSeq |> Seq.skip 1 |> Seq.head)
    
let splitPairs pairs =
    let firsts = pairs |> Seq.map fst
    let seconds = pairs |> Seq.map snd
    (firsts, seconds)
    
let mapDeep func seqSeq =
    seqSeq |> Seq.map (Seq.map func)

let mapMany func seqSeq =
    seqSeq |> Seq.collect func