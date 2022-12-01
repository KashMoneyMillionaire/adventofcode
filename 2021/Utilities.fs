module Utilities

open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let (><) (items1: 'a seq) (items2: 'b seq) =
    Seq.allPairs items1 items2
       
let (>|<) (items1: ('a*'b) seq) (items2: 'c seq) =
    Seq.allPairs items1 items2
    |> Seq.map (fun ((a, b), c) -> (a,b,c))
       
let SplitLinesSplitOn (day: string) (splitBy: char) =
    File.ReadLines("input/" + day + "/input.txt")
    |> Seq.map (fun x -> x.Split(splitBy))
    |> Seq.toList

let ReadInputLines day filename =
    File.ReadLines("input/" + day + "/" + filename)
    |> Seq.skipWhile (fun line -> line = "")
    |> Seq.takeWhile (fun line -> line <> "#")

let WriteLines day filename (lines: string seq) =
    let path = "c:/test/aoc/input/" + day + "/"
    Directory.CreateDirectory path |> ignore
    File.WriteAllLines(path + filename, lines)

let charsAsInt chars = int (String(List.toArray chars))

let toChars x = x |> string |> Seq.toList

let logAndReturn map item =
    printfn (map item)
    item
    
let printLabel label item =
//    printf $"{label}: {item}\n"
    item

let logAndContinue iter map = iter |> Seq.map (logAndReturn map)

let pairwise (offset: int) (source: seq<'T>) =
    let start = seq { yield! source }
    let next = seq { yield! source } |> Seq.skip offset

    Seq.zip start next

let combinePairs (pairs: (_*_) seq) =
    pairs
    |> Seq.map (fun (a, b) -> $"{a}{b}")

let split (splitOn: string) (toSplit: string) =
    toSplit.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)

let chars str = Seq.takeWhile (fun _ -> true) str

type Binary =
    static member parseL(str) = Convert.ToInt64(str, 2)
    
    static member parse(str) = Convert.ToInt32(str, 2)

    static member parse(chars: char seq) =
        chars |> Seq.toArray |> String |> Binary.parse

    static member parse(nums: int seq) =
        nums |> Seq.map (fun n -> (char n) + '0') |> Binary.parse
        
    static member hexToBinary(str: string) =
        let mapping =
            seq {
                ('0', "0000")
                ('1', "0001")
                ('2', "0010")
                ('3', "0011")
                ('4', "0100")
                ('5', "0101")
                ('6', "0110")
                ('7', "0111")
                ('8', "1000")
                ('9', "1001")
                ('a', "1010")
                ('b', "1011")
                ('c', "1100")
                ('d', "1101")
                ('e', "1110")
                ('f', "1111")
                ('A', "1010")
                ('B', "1011")
                ('C', "1100")
                ('D', "1101")
                ('E', "1110")
                ('F', "1111")
            }
            |> dict
        
        str
        |> Seq.map (fun c -> mapping[c])
        |> String.concat ""
        
//        if str.Length <= 16 then
//            let hex = Convert.ToInt64(str, 16)
//            let bin = Convert.ToString(hex, 2)
//            let len = bin.Length
//            String('0', len % 4) + bin
//        else
//            Seq.chunkBySize 16 str
//            |> Seq.map String
//            |> Seq.map Binary.hexToBinary
//            |> String.Concat

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
    |> Seq.mapi (fun y r -> r |> Seq.mapi (fun x -> mapping x y))

let matrixMapL (mapping: int -> int -> 'a -> 'b) (matrix: 'a list list) =
    matrix
    |> List.mapi (fun y r -> r |> List.mapi (fun x -> mapping x y))

let filterMatrix (mapping: int -> int -> 'a -> bool) (matrix: 'a seq seq) =
    matrix
    |> Seq.mapi (fun y r -> r |> Seq.mapi (fun x c -> (mapping x y c, c)))
    |> Seq.map (fun row -> row |> Seq.filter fst |> Seq.map snd)

let filterMatrixL (mapping: int -> int -> 'a -> bool) (matrix: 'a list list) =
    matrix
    |> List.mapi (fun y r -> r |> List.mapi (fun x c -> (mapping x y c, c)))
    |> List.map (fun row -> row |> List.filter fst |> List.map snd)

let inline fst3 (x, _, _) = x
let inline snd3 (_, x, _) = x
let inline third3 (_, _, x) = x

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
  
let (|String|_|) (str: string) = Some str

let (|Char|_|) (str: string) = Some str.[0]

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
    
let unzip pairs =
    let firsts = pairs |> Seq.map fst
    let seconds = pairs |> Seq.map snd
    (firsts, seconds)
    
let mapDeep func seqSeq =
    seqSeq |> Seq.map (Seq.map func)

let mapMany func seqSeq =
    seqSeq |> Seq.collect func
    
let append newItem items =
    items |> Seq.append (Seq.singleton newItem)
    
let filterNot func items=
    items |> Seq.filter (fun i -> not (func i))

let notIn excludedItems items =
    items
    |> filterNot (fun i -> Seq.contains i excludedItems)

let middleItem items =
    items |> List.skip (items.Length / 2) |> List.head
    
let ofLength len item = item |> Seq.length = len

let sortStr (str: string) = str |> Seq.sort |> String.Concat

let findCoordinates (func: 'a -> bool) (matrix: 'a seq seq) =
    matrix
    |> matrixMap (fun x y c -> (x, y, c))
    |> Seq.concat
    |> Seq.filter (fun (_, _, c) -> func c)
    |> Seq.map (fun (x,y,_) -> (x,y))
    
let any items = (Seq.tryHead items).IsSome

let print (matrix: _ seq seq) =
    
    let printItem i =
        printf $"%s{i.ToString()}"
    
    let printRow r =
        r |> Seq.map printItem |> Seq.toList |> ignore
        printf "\n"
    
    matrix |> Seq.map printRow |> Seq.toList |> ignore    
    printf "\n"
    matrix

let printM2 (matrix: _[,]) =
    
    let printItem x y =
        printf $"%s{matrix[x,y].ToString()}"
    
    seq { 0 .. matrix.GetLength(1) - 1 }
    |> Seq.iter (fun y ->
        seq { 0 .. matrix.GetLength(0) - 1 } |> Seq.iter (fun x -> printItem x y)
        printf "\n"
        )
    
    printf "\n"

let printL (matrix: _ list list) =
    
    let printItem i =
        printf $"%s{i.ToString()}"
    
    let printRow r =
        r |> Seq.map printItem |> Seq.toList |> ignore
        printf "\n"
    
    matrix |> Seq.map printRow |> Seq.toList |> ignore    
    printf "\n"
    matrix

let printBy func (matrix: _ seq seq) =
    
    let printItem i =
        printf $"%s{func i}"
    
    let printRow r =
        r |> Seq.map printItem |> Seq.toList |> ignore
        printf "\n"
    
    matrix |> Seq.map printRow |> Seq.toList |> ignore    
    printf "\n"


type MatrixPoint<'a>(x: int, y: int, c: 'a) =

    let mutable top = None
    let mutable bottom = None
    let mutable left = None
    let mutable right = None
    let mutable value = c
    let mutable travelDist = Int32.MaxValue
    let mutable visited = false

    override this.ToString() =
        $"{this.Coordinates} - {this.Value} for dist {this.TravelDist}"

    member this.Coordinates = (x, y)
    
    member this.Visited
        with get () = visited
        and set newVal = visited <- newVal

    member this.Value
        with get () = value
        and set newVal = value <- newVal

    member this.Top
        with get () = top
        and set value = top <- value

    member this.Bottom
        with get () = bottom
        and set value = bottom <- value

    member this.Left
        with get () = left
        and set value = left <- value

    member this.Right
        with get () = right
        and set value = right <- value

    member this.TravelDist
        with get () = travelDist
        and set value = travelDist <- value
        
    member this.TravelStr
        with get () =
            if travelDist = Int32.MaxValue then "  _" else $"%3d{travelDist}"

    member this.Update(newVal: 'a) =
        this.Value <- newVal


type MatrixPointComparer() = 
  interface IComparer<MatrixPoint<int>> with
    member x.Compare(a, b) =
        a.TravelDist.CompareTo(b.TravelDist)

type Matrix<'a> = MatrixPoint<'a> seq seq

let get (coordinates: int*int) (matrix: 'a seq seq) =
    matrix
    |> Seq.skip (snd coordinates)
    |> Seq.head
    |> Seq.skip (fst coordinates)
    |> Seq.head

let size matrix =
    (Seq.length matrix, Seq.length (Seq.head matrix))

let last matrix =
    let l, r = size matrix
    (l-1, r-1)

let buildMatrix (matrix: int seq seq) =
    
    let pairRow (points: MatrixPoint<_> seq) =
        seq {
            use ie = points.GetEnumerator()

            if ie.MoveNext() then
                let mutable prev = ie.Current

                while ie.MoveNext() do
                    let mutable curr = ie.Current

                    prev.Right <- Some curr
                    curr.Left <- Some prev
                    yield prev

                    prev <- curr

                yield prev
        }

    let pairCol (points: MatrixPoint<_> seq) =
        seq {
            use ie = points.GetEnumerator()

            if ie.MoveNext() then
                let mutable prev = ie.Current

                while ie.MoveNext() do
                    let mutable curr = ie.Current

                    prev.Bottom <- Some curr
                    curr.Top <- Some prev
                    yield prev

                    prev <- curr

                yield prev
        }

    matrix
    |> matrixMap (fun x y c -> MatrixPoint<_>(x, y, c))
    |> Seq.map pairRow
    |> Seq.transpose
    |> Seq.map pairCol
    |> Seq.transpose
    |> Seq.cache

let buildMatrixStr (matrix: string seq): MatrixPoint<int> seq seq =
    matrix
    |> Seq.map Seq.toList
    |> mapDeep int
    |> mapDeep (fun i -> i - int '0')
    |> buildMatrix

let optionFilter (seq: _ option seq) =
    seq
    |> Seq.filter (fun i -> i.IsSome)
    |> Seq.map (fun i -> i.Value)
    
let printAll items =
    items |> Seq.iter (fun i -> printf $"{i}")
    
let splitSeq splitOn collection =
    let index = Seq.findIndex (fun i -> i = splitOn) collection
    (
        Seq.take index collection,
        Seq.skip (index + 1) collection
    )
    
let pairMap (leftFunc, rightFunc) (l, r) =
    (leftFunc l, rightFunc r)
    
let pairSeqMap (leftFunc, rightFunc) (seqL, seqR) =
    (
        seqL |> Seq.map leftFunc,
        seqR |> Seq.map rightFunc
    )
    
let getAdjacent (c: MatrixPoint<'a>) includeDiagonal =
    seq {
        yield c.Bottom
        yield c.Top
        yield c.Left
        yield c.Right
        if includeDiagonal then yield c.Right |> Option.bind (fun x -> x.Top)
        if includeDiagonal then yield c.Right |> Option.bind (fun x -> x.Bottom)
        if includeDiagonal then yield c.Left |> Option.bind (fun x -> x.Top)
        if includeDiagonal then yield c.Left |> Option.bind (fun x -> x.Bottom)
    }
    |> optionFilter
    
let listMatrix matrix =
    matrix
    |> Seq.map Seq.toArray
    |> Seq.toArray
    
let except item items =
    items |> Seq.filter (fun i -> i <> item)
    
let matrixAsMap matrix =
    matrix
    |> matrixMap (fun x y c -> ((x,y), c))
    |> Seq.concat
    |> Map

let expandMatrix n func matrix =
    let length, height = size matrix
    let cache = matrixAsMap matrix
    
    seq { 0 .. height * n - 1 }
    |> Seq.map (fun y ->
        { 0 .. length * n - 1 }
        |> Seq.map (fun x ->
            let originalX = x % length
            let originalY = y % height
            let point = cache.[(originalX, originalY)]
            let multiplierX = x / length
            let multiplierY = y / height
            
            func point (multiplierX + multiplierY)
            ))

let stringSeq (str: string) =
    str.ToCharArray()
    |> Array.toSeq

let firstTwo items =
    [|Seq.head items; Seq.head (items |> Seq.skip 1)|]
    
let (%%) (v: int) (modz: int) =
    (v - 1) % modz + 1
    
let seqToList seqSeq =
    seqSeq |> Seq.map Seq.toList |> Seq.toList
    
let either func (l, r) =
    func l || func r
    
let gte testAgainst valToTest =
    valToTest >= testAgainst
    
let toString v =
    v.ToString()
    
let sumBy func cube =
    let mutable count = 0
    cube |> Array3D.iter (fun i -> count <- count + func i)
    count
    
let min3 a b c =
    min (min a b) c
    
let max3 a b c =
    max (max a b) c
    