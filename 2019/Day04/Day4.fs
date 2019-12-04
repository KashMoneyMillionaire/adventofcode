module Day04

open Utilities.Ut

let correctLength (chars: 'a list) = 
    chars.Length = 6

let rec hasDuplicates chars = 
    match chars with
    | [a; b] when a = b -> true
    | [a; b] when a <> b -> false
    | a::b::rest when a = b -> true
    | a::b::rest when a <> b -> hasDuplicates (b::rest)
    
let rec lastRule (runningCount: int) chars = 
    match chars with
    | a::b::c::_ when a = b && b <> c && runningCount = 0 -> true
    | a::b::c::rest when rest.Length > 0 -> lastRule (if a = b then runningCount + 1 else 0) (b::c::rest)
    | [a; b; c;] when a = b && b = c -> false
    | [_;a;b;] when a = b -> true
    | [_;_;_;] -> false
    
let rec neverDecreases chars = 
    match chars with
    | [a; b] when a > b -> false
    | [a; b] when a <= b -> true
    | a::b::rest when a > b -> false
    | a::b::rest when a <= b -> neverDecreases (b::rest)

let charsPassRules chars =
    correctLength chars && hasDuplicates chars && neverDecreases chars

let numsPassRules = numToChars >> charsPassRules

let bruteForce bot top = 
    seq {bot .. top} |> Seq.filter numsPassRules |> Seq.toList

let solve =
    let bot = 357253
    let top = 892942
    let part1 = bruteForce bot top
    let part2 = part1 |> Seq.filter (fun x -> x |> numToChars |> lastRule(0)) |> Seq.toList

    printfn "Part 1: %A" part1.Length
    printfn "Part 2: %A" part2.Length