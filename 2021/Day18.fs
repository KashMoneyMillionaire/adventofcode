module Day18

open System
open Utilities

type Snail =
    | Single of int
    | Pair of Snail * Snail

type ReductionType =
    | Explode
    | Split
    | None

let solve () =

    let grabDigits2 line =
        let nonDigitIdx = line |> Seq.findIndex (fun c -> not (System.Char.IsDigit c))
        let dig = line |> List.take nonDigitIdx |> charsAsInt
        let remaining = line |> List.skip nonDigitIdx
        dig, remaining
    
    let rec parseInput line =
        match line with
        | '[' :: rem ->
            let lSnail, afterSnailL = parseInput rem
            let afterComma = List.tail afterSnailL
            let rSnail, afterSnailR = parseInput afterComma
            (Pair (lSnail, rSnail), List.tail afterSnailR)
        | _ ->
            let dig, remaining = line |> grabDigits2
            Single dig, remaining
    
    let rec findMax =
        function
        | Single i -> i 
        | Pair (l, r) -> max (findMax l) (findMax r)
    
    let rec findDepth =
        function
        | Single _ -> 1
        | Pair (l, r) -> 1 + max (findDepth l) (findDepth r)
    
    let getReductionType (snail: Snail) =
        if findDepth snail > 5 then
            Explode
        else if findMax snail >= 10 then
            Split
        else
            None

    let rec addLeft v =
        function
        | Single i -> Single (v + i)
        | Pair (l, r) -> Pair ((addLeft v l), r)
        
    let rec addRight v =
        function
        | Single i -> Single (v + i)
        | Pair (l, r) -> Pair (l, (addRight v r))
        
    let explode snail =
        let rec ex snail depth =
            match snail with
            | Single _ -> (false, snail, 0, 0)
            | Pair (Single l, Single r) when depth > 4 -> (true, Single 0, l, r) 
            | Pair (Single _, Single _) -> (false, snail, 0, 0) 
            | Pair (l, r) ->
                let isLeftExploded, leftSnail, ll, lr = ex l (depth + 1)
                let isRightExploded, rightSnail, rl, rr = ex r (depth + 1)
                if isLeftExploded then
                    (true, Pair (leftSnail, (addLeft lr r)), ll, 0)
                else if isRightExploded then
                    (true, Pair ((addRight rl l), rightSnail), 0, rr)
                else
                    (false, snail, 0, 0)
        
        ex snail 1 |> snd4
    
    let halfUp i =
        let v = (i / 2.0) |> Math.Ceiling |> int
        Single v
    
    let halfDown i =
        let v = (i / 2.0) |> Math.Floor |> int
        Single v
    
    let rec split snail =
        match snail with
        | Single i -> if i >= 10 then Pair (halfDown i, halfUp i) else snail
        | Pair (l, r) ->
            let newL = split l
            let newR = split r
            if l <> newL then Pair (newL, r)
            else Pair (l, newR)
    
    let rec reduce snail =
        let reductionType = getReductionType snail
        match reductionType with
        | Explode -> reduce (explode snail)
        | Split -> reduce (split snail)
        | None -> snail
    
    let add left right =
        reduce (Pair (left, right))
    
    let rec magnitude =
        function
        | Single i -> i
        | Pair (l, r) -> 3 * magnitude l + 2 * magnitude r
    
    let inputs =
        ReadInputLines "Day18" "input.txt"
        |> Seq.map Seq.toList
        |> Seq.map parseInput
        |> Seq.map fst
    
    let sum =
        inputs
        |> Seq.reduce add

    let magMax =
        inputs >< inputs
        |> Seq.filter (fun (l,r) -> l <> r)
        |> Seq.map (fun (l,r) -> add l r)
        |> mapWith magnitude
        |> Seq.maxBy snd
    
    printfn $"Part 1: {sum}\n{magnitude sum}"
    printfn $"Part 2: {magMax}"
