module Day16

open Utilities

type Instr =
    | Start
    | Version of int
    | Type of int
    | Literal of string
    | Operator of int * Instr seq
    | End

let solve () =

    let LIT = 4
    
    let rec filterToActions instructions =
        instructions
        |> Seq.choose(fun i ->
            match i with
            | Literal _ -> Some i
            | Operator _ -> Some i
            | _ -> None
            )
    
    let parseVersion stream =
        let version = Seq.take 3 stream |> Binary.parse |> printLabel "version"
        (Version version, 3)
    
    let parseType stream =
        let tId = stream |> Seq.take 3 |> Binary.parse |> printLabel "type"
        (Type tId, 3)
    
    let rec parseLiteral stream =
        let instr = stream |> Seq.take 5
        let partialLiteral = instr |> Seq.skip 1 |> Seq.toArray |> System.String |> printLabel "partial literal"
        let nextStream = Seq.skip 5 stream
        
        if Seq.head instr = '0' then
            (Literal partialLiteral, 5)
        else
            let Literal nextLiteral, subBitsConsumed = parseLiteral nextStream
            (Literal $"{partialLiteral}{nextLiteral}", 5 + subBitsConsumed)
    
    let rec parse stream =
        let mutable instr = Start
        let mutable instructions = Seq.empty
        let mutable totalBits = 0
        
        while instr <> End do
            let newInstr, bitsConsumed = parseInstr instr (stream |> Seq.skip totalBits |> Seq.toList) 
            instr <- newInstr
            totalBits <- totalBits + bitsConsumed
            instructions <- (append instr instructions)
            
        (instructions |> filterToActions |> Seq.rev, totalBits)
    
    and parseOp stream opNum =
        let opType = Seq.head stream |> printLabel "opType"
        let mutable totalBitsConsumed = 0
        let mutable opInstructions = Seq.empty

        if opType = '0' then
            let instrBitCount = stream |> Seq.skip 1 |> Seq.take 15 |> Seq.toList |> Binary.parse |> printLabel "op len"
            totalBitsConsumed <- 16
            
            while totalBitsConsumed < instrBitCount + 16 do
                let subInstructions, numBitsConsumed =
                    stream
                    |> Seq.skip (totalBitsConsumed)
                    |> parse
                totalBitsConsumed <- totalBitsConsumed + numBitsConsumed
                opInstructions <- Seq.append subInstructions opInstructions
            
        else 
            let subPackCount = stream |> Seq.skip 1 |> Seq.take 11 |> Binary.parse |> printLabel "op sub count"
            totalBitsConsumed <- 12

            for i = 1 to subPackCount do
                let subInstructions, numBitsConsumed =
                    stream
                    |> Seq.skip (totalBitsConsumed)
                    |> parse
                totalBitsConsumed <- totalBitsConsumed + numBitsConsumed
                opInstructions <- Seq.append subInstructions opInstructions
            
        (Operator (opNum, opInstructions |> Seq.rev), totalBitsConsumed)
    
    and parseInstr (previous: Instr) stream =
        match previous with
        | Start -> parseVersion stream
        | Version _ -> parseType stream
        | Type t when t = LIT -> parseLiteral stream
        | Type t when t <> LIT -> parseOp stream t
        | _ -> (End, 0)
    
    let rec countVersions (instructions: Instr seq) =
        instructions
        |> Seq.map (function 
            | Version v -> v 
            | Operator (_, subInstructions) -> countVersions subInstructions
            | _ -> 0
        )
        |> Seq.sum
    
    let rec printInstr instructions depth =
        instructions
        |> Seq.iter(fun i ->
            let padding = System.String(' ', depth * 2)
            match i with
            | Version v -> printf $"{padding}Version {v}, "
            | Type t -> printf $"Type {t}\n"
            | Literal l -> printf $"{padding}Literal: {Binary.parseL l}\n"
            | Operator (opNum, subs) ->
                printf $"{padding}Op {opNum}:\n"
                printInstr subs (depth + 1)
                printf "\n"
            | _ -> printf ""
            )
    
    let rec eval =
        function
        | Literal l -> Binary.parseL l
        | Operator (0, subs) -> subs |> Seq.map eval |> Seq.sum
        | Operator (1, subs) -> subs |> Seq.map eval |> Seq.toList |> Seq.fold (fun p c -> p * c) 1
        | Operator (2, subs) -> subs |> Seq.map eval |> Seq.min
        | Operator (3, subs) -> subs |> Seq.map eval |> Seq.max
        | Operator (5, subs) ->
            let vals = subs |> Seq.map eval |> Seq.toArray
            if vals[0] > vals[1] then 1 else 0
        | Operator (6, subs) ->
            let vals = subs |> Seq.map eval |> Seq.toArray
            if vals[0] < vals[1] then 1 else 0
        | Operator (7, subs) ->
            let vals = subs |> Seq.map eval |> Seq.toArray
            if vals[0] = vals[1] then 1 else 0
        | _ -> 0
        
    
    let hex = ReadInputLines "Day16" "input.txt" |> Seq.head |> printLabel "hex"
    let binary = hex |> Binary.hexToBinary |> printLabel "binary" |> chars

    let instructions = parse binary |> fst |> Seq.toList
    printInstr instructions 0
    
    let versionSum = countVersions instructions
    let result = eval (instructions |> Seq.head)
    
    printfn $"Part 1: {versionSum}"
    printfn $"Part 2: {result}"
