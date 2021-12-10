module Day08

open Utilities

type Display(letters: string, contains: Display seq) =
    member this.Segments = Seq.toArray letters
    member this.Length = this.Segments.Length
    member this.ContainingDisplays = contains
    member this.Contains display =
        Seq.contains display contains
    
    new(letters: string) =
        Display(letters, Seq.empty)

let solve () =

    let ONE = Display("cf")
    let SEVEN = Display("acf", seq { ONE; })
    let ZERO = Display("abcefg", seq { ONE; SEVEN; })
    
    let TWO = Display("acdeg")
    let THREE = Display("acdfg", seq { ONE; })
    let FOUR = Display("bcdf", seq { ONE; })
    let FIVE = Display("abdfg")
    let SIX = Display("abdefg", seq { FIVE; })
    let NINE = Display("abcdfg", seq { THREE; FOUR; FIVE; })
    let EIGHT = Display("abcdefg", seq { ZERO; ONE; TWO; THREE; FOUR; FIVE; SIX; SEVEN; NINE; })

    let nums = seq {
        yield! EIGHT.ContainingDisplays; EIGHT
    }
    
    let uniqueLengths =
        seq {
            2; 3; 4; 7
        }
//
//
//    let findPossible (patterns: string seq) =
//        patterns
//        |> mapWith (fun p -> nums |> Seq.filter (fun d -> d.Length = p.Length))
//    
//    let diff (matches: (string * Display) seq) (item: string * Display seq) =
//        let disp = snd item
//        
//        matches
//        |> 
//        1
//    
//    let subsetChars (previouslySolved: Map<char, char>) (matches: (string * Display) seq) =
//        let newSolved =
//            matches
//            |> Seq.map (diff matches)
//        
//        previouslySolved
//        |> Seq.append newSolved
//    
//    let rec reduceSolve (patterns: string[]) previouslySolved =
//        
//        let possibilities = patterns |> findPossible
//        let matches = possibilities
//                      |> Seq.filter (fun p -> Seq.length(snd p) = 1)
//                      |> Seq.map (fun p -> (fst p, Seq.head(snd p)))
//        
//        let solved = subsetChars previouslySolved matches
//        
//        let knownCharacters = Map.empty<char, char>
//        
//        if Seq.length possibilities = Seq.length matches
//        then
//            matches |> Seq.map (fun s -> (fst s, Seq.head (snd s)))
//        else
//            reduceSolve patterns matches
//    
//    let rec solveLine (patterns: string[], theFour: string[]) =
//        
//        let solvedLine = reduceSolve patterns Seq.empty
//            
//        
//        1
//    
    let signals, outputs =
        ReadInputLines "Day08" "input.txt"
        |> Seq.takeWhile (fun line -> line <> "#")
        |> Seq.map (split " | ")
        |> Seq.map paired
        |> Seq.map (fun p -> (split "" (fst p), split "" (snd p)))
        |> splitPairs

    let uniques =
        outputs
        |> mapDeep (fun item -> item.Length)
        |> mapMany id
        |> Seq.filter (fun len -> Seq.contains len uniqueLengths)
        |> Seq.length

//    let solved =
//        Seq.zip signals outputs
//        |> Seq.map solveLine
    
    printfn $"Part 1: {uniques}"
    printfn $"Part 2: "
